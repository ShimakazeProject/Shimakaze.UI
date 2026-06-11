using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Silk.NET.Core.Contexts;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;

using SkiaSharp;

namespace Shimakaze.UI.Rendering.Vulkan;

internal sealed class VulkanApplication : IDisposable
{
    private static bool s_enableValidationLayers = false;
    private static readonly string[] ValidationLayers =
    [
        "VK_LAYER_KHRONOS_validation",
    ];

    public readonly Vk Vk;
    private readonly IVkSurface _vkSurface;
    private readonly string? _applicationName;
    private readonly string[] _instanceExtensions = [];
    private readonly string[] _deviceExtensions = [
        KhrSwapchain.ExtensionName,
    ];

    private ExtDebugUtils? _extDebugUtils;
    private KhrSurface _khrSurface;

    public Instance Instance;
    public DebugUtilsMessengerEXT DebugMessenger;
    public SurfaceKHR Surface;
    public PhysicalDevice PhysicalDevice;
    public QueueFamilyIndices Indices;
    public Device Device;
    public Queue GraphicsQueue;
    public Queue PresentQueue;
    public SwapChainSupportDetails SwapChainSupport;

    public GRContext GRContext;

    private bool _disposedValue;

    static VulkanApplication()
    {
        InitializeEnableValidationLayers();
    }

    public VulkanApplication(IVkSurface vkSurface, string? applicationName = null)
    {
        _vkSurface = vkSurface;
        _applicationName = applicationName;
        Vk = Vk.GetApi();

        _instanceExtensions = GetRequiredExtensions();

        DebugPrintInstanceExtensions();

        CreateInstance();
        SetupDebugMessenger();
        CreateSurface();
        PickPhysicalDevice();
        CreateLogicalDevice();

        CreateGRContext();
    }

    [MemberNotNull(nameof(Instance))]
    private unsafe void CreateInstance()
    {
        if (s_enableValidationLayers)
            s_enableValidationLayers = CheckValidationLayerSupport();

        List<IDisposable> disposables = [];
        try
        {
            var memory = SilkMarshal.StringToMemory("Shimakaze.UI (Vulkan)", NativeStringEncoding.UTF8);
            disposables.Add(memory);
            var pEngingeName = memory.AsPtr<byte>(0);

            byte* pApplicationName = null;
            if (string.IsNullOrEmpty(_applicationName))
            {
                memory = SilkMarshal.StringToMemory(_applicationName, NativeStringEncoding.UTF8);
                disposables.Add(memory);
                pApplicationName = memory.AsPtr<byte>(0);
            }

            ApplicationInfo appInfo = new()
            {
                SType = StructureType.ApplicationInfo,
                PApplicationName = pApplicationName,
                ApplicationVersion = Vk.Version10,
                PEngineName = pEngingeName,
                EngineVersion = Vk.Version10,
                ApiVersion = Vk.Version13,
            };

            InstanceCreateInfo createInfo = new()
            {
                SType = StructureType.InstanceCreateInfo,
                Flags = InstanceCreateFlags.None,
                PApplicationInfo = &appInfo,
            };

            DebugUtilsMessengerCreateInfoEXT debugCreateInfo;
            if (s_enableValidationLayers)
            {
                memory = SilkMarshal.StringArrayToMemory([.. ValidationLayers], NativeStringEncoding.UTF8);
                disposables.Add(memory);

                createInfo.EnabledLayerCount = (uint)ValidationLayers.Length;
                createInfo.PpEnabledLayerNames = (byte**)memory.Handle;

                PopulateDebugMessengerCreateInfo(out debugCreateInfo);
                createInfo.PNext = &debugCreateInfo;
            }

            if (_instanceExtensions is { Length: not 0 } extensions)
            {
                memory = SilkMarshal.StringArrayToMemory(extensions, NativeStringEncoding.UTF8);
                disposables.Add(memory);
                createInfo.EnabledExtensionCount = (uint)extensions.Length;
                createInfo.PpEnabledExtensionNames = (byte**)memory.Handle;
            }

            var result = Vk.CreateInstance(ref createInfo, null, out Instance);
            if (result is Result.ErrorIncompatibleDriver)
                throw new NotSupportedException("VK_ERROR_INCOMPATIBLE_DRIVER");

            if (result is Result.ErrorExtensionNotPresent)
                throw new NotSupportedException("VK_ERROR_EXTENSION_NOT_PRESENT");

            result.EnsureSuccessed("failed to create instance!");

            Debug.Assert(Instance.Handle is not 0);
        }
        finally
        {
            disposables.Reverse();
            foreach (var item in disposables)
                item.Dispose();
        }
    }

    private unsafe void SetupDebugMessenger()
    {
        if (!s_enableValidationLayers)
            return;

        if (!Vk.TryGetInstanceExtension(Instance, out _extDebugUtils) || _extDebugUtils is null)
            throw new NotSupportedException(ExtDebugUtils.ExtensionName);

        PopulateDebugMessengerCreateInfo(out var createInfo);

        _extDebugUtils.CreateDebugUtilsMessenger(Instance, ref createInfo, null, out DebugMessenger)
            .EnsureSuccessed("failed to set up debug messenger!");
    }

    [MemberNotNull(nameof(_khrSurface), nameof(Surface))]
    private unsafe void CreateSurface()
    {
        if (!Vk.TryGetInstanceExtension(Instance, out _khrSurface) || _khrSurface is null)
            throw new NotSupportedException(KhrSurface.ExtensionName);

        Surface = _vkSurface.Create<AllocationCallbacks>(Instance.ToHandle(), null).ToSurface();
    }


    [MemberNotNull(nameof(PhysicalDevice), nameof(Indices), nameof(SwapChainSupport))]
    private void PickPhysicalDevice()
    {
        var devices = Vk.GetPhysicalDevices(Instance);
        if (devices is null or { Count: 0 })
            throw new InvalidProgramException("failed to find GPUs with Vulkan support!");

        var tmp1 = devices
            .Select(i => (rate: RateDeviceSuitability(i), device: i))
            .Where(i => i.rate > 0);

        if (!tmp1.Any())
            throw new InvalidProgramException("failed to find a suitable GPU!");

        PhysicalDevice = tmp1
            .MaxBy(i => i.rate)
            .device;

        Indices = FindQueueFamilies(PhysicalDevice);
        SwapChainSupport = QuerySwapChainSupport(PhysicalDevice);
    }


    private int RateDeviceSuitability(PhysicalDevice device)
    {
        var indices = FindQueueFamilies(device);
        if (!indices.IsComplete)
            return 0;

        if (!CheckDeviceExtensionSupport(device))
            return 0;

        SwapChainSupport = QuerySwapChainSupport(device);
        if (SwapChainSupport is not { Formats.Count: not 0, PresentModes.Count: not 0 })
            return 0;

        Vk.GetPhysicalDeviceProperties(device, out var deviceProperties);
        Vk.GetPhysicalDeviceFeatures(device, out var deviceFeatures);

        int score = 0;

        // Discrete GPUs have a significant performance advantage
        if (deviceProperties.DeviceType == PhysicalDeviceType.DiscreteGpu)
            score += 1000;

        // Maximum possible size of textures affects graphics quality
        score += (int)deviceProperties.Limits.MaxImageDimension2D;

        // Application can't function without geometry shaders
        if (!deviceFeatures.GeometryShader)
            return 0;

        return score;
    }

    private QueueFamilyIndices FindQueueFamilies(PhysicalDevice device)
    {
        Debug.Assert(_khrSurface is not null);

        QueueFamilyIndices indices = new();

        var queueFamilies = Utils.TwoStep<QueueFamilyProperties>((ref r, ref c) => Vk.GetPhysicalDeviceQueueFamilyProperties(device, ref c, out r));

        uint i = 0;
        foreach (var queueFamily in queueFamilies)
        {
            if (queueFamily.QueueFlags.HasFlag(QueueFlags.GraphicsBit))
                indices.GraphicsFamily = i;

            _khrSurface.GetPhysicalDeviceSurfaceSupport(device, i, Surface, out var presentSupport);

            if (presentSupport)
                indices.PresentFamily = i;

            if (indices.IsComplete)
                break;

            i++;
        }


        return indices;
    }

    private unsafe bool CheckDeviceExtensionSupport(PhysicalDevice device)
    {
        var availableExtensions = Utils.TwoStep<ExtensionProperties>((ref r, ref c) => Vk.EnumerateDeviceExtensionProperties(device, (byte*)null, ref c, ref r).EnsureSuccessed());
        List<string> requiredExtensions = [.. _deviceExtensions];

        foreach (var extension in availableExtensions)
        {
            var name = SilkMarshal.PtrToString((nint)extension.ExtensionName, NativeStringEncoding.UTF8);
            if (name is not null && requiredExtensions.Contains(name))
                requiredExtensions.Remove(name);
        }

        return requiredExtensions.Count is 0;
    }

    private SwapChainSupportDetails QuerySwapChainSupport(PhysicalDevice device)
    {
        Debug.Assert(_khrSurface is not null);
        SwapChainSupportDetails details = new();

        _khrSurface.GetPhysicalDeviceSurfaceCapabilities(device, Surface, out var capabilities);
        details.Capabilities = capabilities;

        details.Formats = Utils.TwoStep<SurfaceFormatKHR>((ref r, ref c) => _khrSurface.GetPhysicalDeviceSurfaceFormats(device, Surface, ref c, out r).EnsureSuccessed());
        details.PresentModes = Utils.TwoStep<PresentModeKHR>((ref r, ref c) => _khrSurface.GetPhysicalDeviceSurfacePresentModes(device, Surface, ref c, out r).EnsureSuccessed());

        return details;
    }


    [MemberNotNull(nameof(Device), nameof(GraphicsQueue), nameof(PresentQueue))]
    private unsafe void CreateLogicalDevice()
    {
        Debug.Assert(Indices?.IsComplete is true);

        List<DeviceQueueCreateInfo> queueCreateInfoList = [];
        float queuePriority = 1.0f;
        foreach (var queueFamily in new[] {
                Indices.GraphicsFamily.Value,
                Indices.PresentFamily.Value
            }.Distinct())
        {
            DeviceQueueCreateInfo queueCreateInfo = new()
            {
                SType = StructureType.DeviceQueueCreateInfo,
                QueueFamilyIndex = queueFamily,
                QueueCount = 1,

                PQueuePriorities = &queuePriority,
            };

            queueCreateInfoList.Add(queueCreateInfo);
        }

        PhysicalDeviceFeatures deviceFeatures = new();

        fixed (DeviceQueueCreateInfo* ptr = queueCreateInfoList.ToArray())
        {
            DeviceCreateInfo createInfo = new()
            {
                SType = StructureType.DeviceCreateInfo,
                PQueueCreateInfos = ptr,
                QueueCreateInfoCount = (uint)queueCreateInfoList.Count,

                PEnabledFeatures = &deviceFeatures,
            };

            GlobalMemory? memory = null;
            try
            {
                if (_deviceExtensions is { Length: not 0 } extensions)
                {
                    memory = SilkMarshal.StringArrayToMemory(extensions, NativeStringEncoding.UTF8);
                    createInfo.EnabledExtensionCount = (uint)extensions.Length;
                    createInfo.PpEnabledExtensionNames = (byte**)memory.Handle;
                }

                Vk.CreateDevice(PhysicalDevice, ref createInfo, null, out Device)
                    .EnsureSuccessed("failed to create logical device!");
            }
            finally
            {
                memory?.Dispose();
            }
        }

        GraphicsQueue = Vk.GetDeviceQueue(Device, Indices.GraphicsFamily.Value, 0);
        PresentQueue = Vk.GetDeviceQueue(Device, Indices.PresentFamily.Value, 0);

    }

    [MemberNotNull(nameof(GRContext))]
    private void CreateGRContext()
    {
        Debug.Assert(Indices.IsComplete);

        GRVkBackendContext grVkBackendContext = new()
        {
            VkInstance = Instance.Handle,
            VkPhysicalDevice = PhysicalDevice.Handle,
            VkDevice = Device.Handle,
            VkQueue = GraphicsQueue.Handle,
            GraphicsQueueIndex = Indices.GraphicsFamily.Value,
            MaxAPIVersion = Vk.Version13,
            GetProcedureAddress = (name, instance, device) =>
            {
                Debug.WriteLine($"GetProcedureAddress: {name}, instance=0x{instance:X8}, device=0x{device:X8}");
                if (device is not 0)
                {
                    if (device == Device.Handle)
                    {
                        var p = Vk.GetDeviceProcAddr(Device, name);

                        if (p != 0)
                            return p;
                    }
                    else if (instance is 0)
                    {
                        instance = Instance.Handle;
                    }
                }

                // vkEnumerateInstanceExtensionProperties need keep instance as 0.
                return Vk.GetInstanceProcAddr(new(instance), name);
            },
        };
        grVkBackendContext.Extensions = GRVkExtensions.Create(grVkBackendContext.GetProcedureAddress, grVkBackendContext.VkInstance, grVkBackendContext.VkPhysicalDevice, _instanceExtensions, _deviceExtensions);

        GRContext = GRContext.CreateVulkan(grVkBackendContext);
    }

    private unsafe string[] GetRequiredExtensions()
    {
        var ptr = _vkSurface.GetRequiredExtensions(out var count);
        List<string> extensions = [.. SilkMarshal.PtrToStringArray((nint)ptr, (int)count, NativeStringEncoding.UTF8)];

        if (s_enableValidationLayers)
            extensions.Add(ExtDebugUtils.ExtensionName);

        return [.. extensions];
    }

    private unsafe bool CheckValidationLayerSupport()
    {
        var availableLayers = Utils.TwoStep<LayerProperties>((ref r, ref c) => Vk.EnumerateInstanceLayerProperties(ref c, ref r).EnsureSuccessed());

        Debug.Print(availableLayers);

        foreach (var layerName in ValidationLayers)
        {
            bool layerFound = false;

            foreach (var layerProperties in availableLayers)
            {
                var name = SilkMarshal.PtrToString((nint)layerProperties.LayerName, NativeStringEncoding.UTF8);
                if (name == layerName)
                {
                    layerFound = true;
                    break;
                }
            }

            if (!layerFound)
                return false;
        }

        return true;
    }

    [Conditional("DEBUG")]
    private void DebugPrintInstanceExtensions()
    {
        var extensions = Utils.TwoStep<ExtensionProperties>((ref r, ref c) => Vk.EnumerateInstanceExtensionProperties((string?)null, ref c, ref r).EnsureSuccessed());

        Debug.Print(extensions);
    }

    private static unsafe void PopulateDebugMessengerCreateInfo(out DebugUtilsMessengerCreateInfoEXT createInfo) => createInfo = new()
    {
        SType = StructureType.DebugUtilsMessengerCreateInfoExt,
        MessageSeverity = DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt
                | DebugUtilsMessageSeverityFlagsEXT.InfoBitExt
                | DebugUtilsMessageSeverityFlagsEXT.WarningBitExt
                | DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt,
        MessageType = DebugUtilsMessageTypeFlagsEXT.GeneralBitExt
                | DebugUtilsMessageTypeFlagsEXT.ValidationBitExt
                | DebugUtilsMessageTypeFlagsEXT.PerformanceBitExt,
        PfnUserCallback = new(DebugCallback),
        PUserData = null,
    };

    private static unsafe uint DebugCallback(
        DebugUtilsMessageSeverityFlagsEXT messageSeverity,
        DebugUtilsMessageTypeFlagsEXT messageType,
        DebugUtilsMessengerCallbackDataEXT* pCallbackData,
        void* pUserData)
    {
        var msg = SilkMarshal.PtrToString((nint)pCallbackData->PMessage, NativeStringEncoding.UTF8);
        Debug.WriteLine($"validation layer: {msg}");
        Console.WriteLine($"validation layer: {msg}");

        return 0;
    }


    [Conditional("DEBUG")]
    private static void InitializeEnableValidationLayers()
    {
        s_enableValidationLayers = true;
    }

    public unsafe void Dispose()
    {
        if (_disposedValue)
            return;
        _disposedValue = true;

        // 1. Dispose GRContext first (it references VkDevice/Queue internally)
        GRContext?.Dispose();
        GRContext = null!;

        // 2. Destroy logical device
        if (Device.Handle is not 0)
        {
            Vk.DestroyDevice(Device, null);
            Device = default;
        }

        // 3. Destroy surface
        if (Surface.Handle is not 0 && _khrSurface is not null)
        {
            _khrSurface.DestroySurface(Instance, Surface, null);
            Surface = default;
            _khrSurface.Dispose();
            _khrSurface = null!;
        }

        // 4. Destroy debug messenger (if validation layers enabled)
        if (s_enableValidationLayers && DebugMessenger.Handle is not 0 && _extDebugUtils is not null)
        {
            _extDebugUtils.DestroyDebugUtilsMessenger(Instance, DebugMessenger, null);
            DebugMessenger = default;
        }

        // 5. Destroy instance (last, after all child objects are destroyed)
        if (Instance.Handle is not 0)
        {
            Vk.DestroyInstance(Instance, null);
            Instance = default;
        }

        // 6. Dispose Vulkan API handle
        Vk?.Dispose();

        GC.SuppressFinalize(this);
    }

    ~VulkanApplication()
    {
        Dispose();
    }
}