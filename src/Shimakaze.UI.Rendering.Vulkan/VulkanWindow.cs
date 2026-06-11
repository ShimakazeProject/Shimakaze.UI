using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;

using SkiaSharp;

using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace Shimakaze.UI.Rendering.Vulkan;

internal sealed class VulkanWindow
{
    private const int MAX_FRAMES_IN_FLIGHT = 2;

    private readonly IWindow _window;
    private readonly VulkanApplication _application;
    private readonly KhrSwapchain _khrSwapchain;

    private readonly Fence[] _inFlightFences = GC.AllocateUninitializedArray<Fence>(MAX_FRAMES_IN_FLIGHT);
    private readonly Semaphore[] _imageAvailableSemaphores = GC.AllocateUninitializedArray<Semaphore>(MAX_FRAMES_IN_FLIGHT);
    private readonly Semaphore[] _renderFinishedSemaphores = GC.AllocateUninitializedArray<Semaphore>(MAX_FRAMES_IN_FLIGHT);

    private SwapchainKHR _swapchain;
    private ImageUsageFlags _swapchainUsageFlags;
    private Image[]? _swapChainImages;
    private Format _swapChainImageFormat;
    private Extent2D _swapChainExtent;

    private SKSurface[]? _surfaces;

    public unsafe VulkanWindow(IWindow window, VulkanApplication application)
    {
        _window = window;
        _application = application;

        if (!_application.Vk.TryGetDeviceExtension(_application.Instance, _application.Device, out _khrSwapchain))
            throw new NotSupportedException(KhrSwapchain.ExtensionName);

        Debug.Assert(_khrSwapchain is not null);

        for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
        {
            SemaphoreCreateInfo semaphoneCreateInfo = new()
            {
                SType = StructureType.SemaphoreCreateInfo,
                Flags = SemaphoreCreateFlags.None,
            };

            application.Vk.CreateSemaphore(application.Device, ref semaphoneCreateInfo, null, out _imageAvailableSemaphores[i]).EnsureSuccessed();
            application.Vk.CreateSemaphore(application.Device, ref semaphoneCreateInfo, null, out _renderFinishedSemaphores[i]).EnsureSuccessed();

            FenceCreateInfo fenceCreateInfo = new()
            {
                SType = StructureType.FenceCreateInfo,
                Flags = FenceCreateFlags.SignaledBit,
            };

            application.Vk.CreateFence(application.Device, ref fenceCreateInfo, null, out _inFlightFences[i]).EnsureSuccessed();
        }
    }
    private unsafe void CleanupSwapChain()
    {
        _khrSwapchain.DestroySwapchain(_application.Device, _swapchain, null);
    }
    public void RecreateSwapChain()
    {
        _application.Vk.DeviceWaitIdle(_application.Device);

        CleanupSwapChain();

        CreateSwapChain();
    }

    [MemberNotNull(nameof(_khrSwapchain), nameof(_swapchain), nameof(_swapchainUsageFlags), nameof(_surfaces), nameof(_swapChainImages))]
    public unsafe void CreateSwapChain()
    {
        Debug.Assert(_application.Indices.IsComplete is true);

        var swapChainSupport = _application.SwapChainSupport;

        var extent = ChooseSwapExtent(swapChainSupport.Capabilities);
        if ((extent.Width, extent.Height) == (_swapChainExtent.Width, _swapChainExtent.Height))
        {
            Debug.Assert(_khrSwapchain is not null);
            Debug.Assert(_surfaces is not null);
            Debug.Assert(_swapChainImages is not null);
            return;
        }

        _swapChainExtent = extent;

        var surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
        var presentMode = ChooseSwapPresentMode(swapChainSupport.PresentModes);

        var imageCount = uint.Clamp(swapChainSupport.Capabilities.MinImageCount + 1, swapChainSupport.Capabilities.MinImageCount, swapChainSupport.Capabilities.MaxImageCount);

        var queueFamilyIndex = _application.Indices.GraphicsFamily.Value;

        _swapchainUsageFlags = ImageUsageFlags.ColorAttachmentBit
            | ImageUsageFlags.TransferSrcBit
            | ImageUsageFlags.TransferDstBit
            | ImageUsageFlags.SampledBit;

        SwapchainCreateInfoKHR createInfo = new()
        {
            SType = StructureType.SwapchainCreateInfoKhr,
            Surface = _application.Surface,
            MinImageCount = imageCount,
            ImageFormat = surfaceFormat.Format,
            ImageColorSpace = surfaceFormat.ColorSpace,
            ImageExtent = extent,
            ImageArrayLayers = 1,
            ImageUsage = _swapchainUsageFlags,
            ImageSharingMode = SharingMode.Exclusive,
            QueueFamilyIndexCount = 1,
            PQueueFamilyIndices = &queueFamilyIndex,
            PreTransform = SurfaceTransformFlagsKHR.IdentityBitKhr,
            CompositeAlpha = CompositeAlphaFlagsKHR.InheritBitKhr,
            PresentMode = presentMode,
            Clipped = true,
            OldSwapchain = default,
        };

        uint[] queueFamilyIndices = [_application.Indices.GraphicsFamily.Value, _application.Indices.PresentFamily.Value];
        fixed (uint* ptr = queueFamilyIndices)
        {
            if (_application.Indices.GraphicsFamily != _application.Indices.PresentFamily)
            {
                createInfo.ImageSharingMode = SharingMode.Concurrent;
                createInfo.QueueFamilyIndexCount = 2;
                createInfo.PQueueFamilyIndices = ptr;
            }
            else
            {
                createInfo.ImageSharingMode = SharingMode.Exclusive;
                createInfo.QueueFamilyIndexCount = 0; // Optional
                createInfo.PQueueFamilyIndices = null; // Optional
            }

            createInfo.PreTransform = swapChainSupport.Capabilities.CurrentTransform;
            createInfo.CompositeAlpha = CompositeAlphaFlagsKHR.OpaqueBitKhr;
            createInfo.PresentMode = presentMode;
            createInfo.Clipped = true;
            createInfo.OldSwapchain = default;

            _khrSwapchain.CreateSwapchain(_application.Device, ref createInfo, null, out _swapchain).EnsureSuccessed("failed to create swap chain!");
        }

        _swapChainImages = Utils.TwoStep<Image>((ref r, ref c) => _khrSwapchain.GetSwapchainImages(_application.Device, _swapchain, ref c, out r).EnsureSuccessed());
        _surfaces = new SKSurface[_swapChainImages.Length];

        _swapChainImageFormat = surfaceFormat.Format;
    }

    private Extent2D ChooseSwapExtent(in SurfaceCapabilitiesKHR capabilities)
    {
        if (capabilities.CurrentExtent.Width is not uint.MaxValue)
            return capabilities.CurrentExtent;

        uint width = uint.Clamp((uint)_window.FramebufferSize.X, capabilities.MinImageExtent.Width, capabilities.MaxImageExtent.Width);
        uint height = uint.Clamp((uint)_window.FramebufferSize.Y, capabilities.MinImageExtent.Height, capabilities.MaxImageExtent.Height);

        return new(width, height);
    }

    private static PresentModeKHR ChooseSwapPresentMode(IEnumerable<PresentModeKHR> availablePresentModes)
    {
        foreach (var availablePresentMode in availablePresentModes)
        {
            if (availablePresentMode is PresentModeKHR.MailboxKhr)
                return availablePresentMode;
        }

        return PresentModeKHR.FifoKhr;
    }

    private static SurfaceFormatKHR ChooseSwapSurfaceFormat(IEnumerable<SurfaceFormatKHR> availableFormats)
    {
        foreach (var availableFormat in availableFormats)
        {
            if (availableFormat is { Format: Format.B8G8R8A8Unorm, ColorSpace: ColorSpaceKHR.SpaceSrgbNonlinearKhr })
                return availableFormat;
        }

        return availableFormats.First();
    }

    public SKSurface GetCurrent(ulong currentFrame)
    {
        Debug.Assert(_khrSwapchain is not null);
        Debug.Assert(_swapChainImages is not null);
        Debug.Assert(!_application.GRContext.IsAbandoned);
        Debug.Assert(_application.Indices.IsComplete);

        var frame = currentFrame % MAX_FRAMES_IN_FLIGHT;

        _application.Vk.WaitForFences(_application.Device, 1, ref _inFlightFences[frame], true, ulong.MaxValue).EnsureSuccessed();

        _application.Vk.ResetFences(_application.Device, 1, ref _inFlightFences[frame]).EnsureSuccessed();

        uint imageIndex = 0;
        _khrSwapchain.AcquireNextImage(
            _application.Device,
            _swapchain,
            ulong.MaxValue,
            _imageAvailableSemaphores[frame],
            default,
            ref imageIndex)
            .EnsureSuccessed();

        var image = _swapChainImages[imageIndex];

        Debug.Assert(image.Handle is not 0);

        GRVkAlloc alloc = new();
        GRVkImageInfo imageInfo = new()
        {
            Image = image.Handle,
            ImageTiling = (uint)ImageTiling.Optimal,
            ImageLayout = (uint)ImageLayout.Undefined,
            Format = (uint)_swapChainImageFormat,
            ImageUsageFlags = (uint)_swapchainUsageFlags,
            SampleCount = 1,
            LevelCount = 1,
            CurrentQueueFamily = _application.Indices.GraphicsFamily.Value,
            SharingMode = (uint)(_application.Indices.GraphicsFamily != _application.Indices.PresentFamily
                ? SharingMode.Concurrent
                : SharingMode.Exclusive),
            Alloc = alloc,
        };

        GRBackendRenderTarget backendRenderTarget = new((int)_swapChainExtent.Width, (int)_swapChainExtent.Height, imageInfo);
        Debug.Assert(backendRenderTarget.IsValid);

        var surface = SKSurface.Create(_application.GRContext, backendRenderTarget, GRSurfaceOrigin.TopLeft, SKColorType.Bgra8888);

        Debug.Assert(!_application.GRContext.IsAbandoned);
        Debug.Assert(surface is not null);

        return surface;
    }

}