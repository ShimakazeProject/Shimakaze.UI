using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;

using SkiaSharp;

using Semaphore = Silk.NET.Vulkan.Semaphore;

namespace Shimakaze.UI.Rendering.Vulkan;

internal sealed class VulkanWindow : IDisposable
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

    private ImageLayout[]? _imageLayouts;
    private uint _currentImageIndex;
    private int _currentFrameIndex;

    private CommandPool _commandPool;
    private CommandBuffer _commandBuffer;

    private bool _disposed;

    public unsafe VulkanWindow(IWindow window, VulkanApplication application)
    {
        _window = window;
        _application = application;

        if (!_application.Vk.TryGetDeviceExtension(_application.Instance, _application.Device, out _khrSwapchain))
            throw new NotSupportedException(KhrSwapchain.ExtensionName);

        Debug.Assert(_khrSwapchain is not null);

        CreateSyncObjects();
        CreateCommandPool();
        AllocateCommandBuffer();
    }

    private unsafe void CreateSyncObjects()
    {
        for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
        {
            SemaphoreCreateInfo semaphoreCreateInfo = new()
            {
                SType = StructureType.SemaphoreCreateInfo,
                Flags = SemaphoreCreateFlags.None,
            };

            _application.Vk.CreateSemaphore(_application.Device, ref semaphoreCreateInfo, null, out _imageAvailableSemaphores[i]).EnsureSuccessed();
            _application.Vk.CreateSemaphore(_application.Device, ref semaphoreCreateInfo, null, out _renderFinishedSemaphores[i]).EnsureSuccessed();

            FenceCreateInfo fenceCreateInfo = new()
            {
                SType = StructureType.FenceCreateInfo,
                Flags = FenceCreateFlags.SignaledBit,
            };

            _application.Vk.CreateFence(_application.Device, ref fenceCreateInfo, null, out _inFlightFences[i]).EnsureSuccessed();
        }
    }

    private unsafe void CreateCommandPool()
    {
        CommandPoolCreateInfo poolInfo = new()
        {
            SType = StructureType.CommandPoolCreateInfo,
            Flags = CommandPoolCreateFlags.ResetCommandBufferBit,
            QueueFamilyIndex = _application.Indices.GraphicsFamily!.Value,
        };

        _application.Vk.CreateCommandPool(_application.Device, ref poolInfo, null, out _commandPool)
            .EnsureSuccessed("failed to create command pool!");
    }

    private unsafe void AllocateCommandBuffer()
    {
        CommandBufferAllocateInfo allocInfo = new()
        {
            SType = StructureType.CommandBufferAllocateInfo,
            CommandPool = _commandPool,
            Level = CommandBufferLevel.Primary,
            CommandBufferCount = 1,
        };

        fixed (CommandBuffer* pCommandBuffer = &_commandBuffer)
        {
            _application.Vk.AllocateCommandBuffers(_application.Device, ref allocInfo, pCommandBuffer)
                .EnsureSuccessed("failed to allocate command buffer!");
        }
    }

    private unsafe void CleanupSwapChain()
    {
        if (_surfaces is { Length: not 0 })
        {
            foreach (var item in _surfaces)
                item.Dispose();

            _surfaces = null;
        }

        _imageLayouts = null;

        if (_swapchain.Handle is not 0)
        {
            _khrSwapchain.DestroySwapchain(_application.Device, _swapchain, null);
            _swapchain = default;
        }
    }

    public void RecreateSwapChain()
    {
        _application.Vk.DeviceWaitIdle(_application.Device);

        CleanupSwapChain();

        CreateSwapChain();
    }

    [MemberNotNull(nameof(_swapchain), nameof(_swapchainUsageFlags), nameof(_imageLayouts), nameof(_swapChainImages))]
    public unsafe void CreateSwapChain()
    {
        Debug.Assert(_application.Indices.IsComplete is true);

        var swapChainSupport = _application.SwapChainSupport;

        var surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);
        var presentMode = ChooseSwapPresentMode(swapChainSupport.PresentModes);
        var extent = ChooseSwapExtent(swapChainSupport.Capabilities);

        _swapChainExtent = extent;

        var imageCount = uint.Clamp(
            swapChainSupport.Capabilities.MinImageCount + 1,
            swapChainSupport.Capabilities.MinImageCount,
            swapChainSupport.Capabilities.MaxImageCount > 0
                ? swapChainSupport.Capabilities.MaxImageCount
                : uint.MaxValue);

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
            PreTransform = swapChainSupport.Capabilities.CurrentTransform,
            CompositeAlpha = CompositeAlphaFlagsKHR.OpaqueBitKhr,
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

            _khrSwapchain.CreateSwapchain(_application.Device, ref createInfo, null, out _swapchain)
                .EnsureSuccessed("failed to create swap chain!");
        }

        _swapChainImages = Utils.TwoStep<Image>((ref r, ref c) =>
            _khrSwapchain.GetSwapchainImages(_application.Device, _swapchain, ref c, out r).EnsureSuccessed());

        _surfaces = new SKSurface[_swapChainImages.Length];

        _imageLayouts = new ImageLayout[_swapChainImages.Length];
        // All swapchain images start in Undefined layout
        Array.Fill(_imageLayouts, ImageLayout.Undefined);
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

    public unsafe SKSurface GetCurrent(ulong currentFrame)
    {
        Debug.Assert(_khrSwapchain is not null);
        Debug.Assert(_swapChainImages is not null);
        Debug.Assert(_imageLayouts is not null);
        Debug.Assert(_surfaces is not null);
        Debug.Assert(!_application.GRContext.IsAbandoned);
        Debug.Assert(_application.Indices.IsComplete);

        var frame = (int)(currentFrame % MAX_FRAMES_IN_FLIGHT);

        // 1. Wait for the previous frame's GPU work to complete
        _application.Vk.WaitForFences(_application.Device, 1, ref _inFlightFences[frame], true, ulong.MaxValue)
            .EnsureSuccessed();

        _application.Vk.ResetFences(_application.Device, 1, ref _inFlightFences[frame])
            .EnsureSuccessed();

        // 2. Acquire the next swapchain image
        uint imageIndex = 0;
        var acquireResult = _khrSwapchain.AcquireNextImage(
            _application.Device,
            _swapchain,
            ulong.MaxValue,
            _imageAvailableSemaphores[frame],
            default,
            ref imageIndex);

        if (acquireResult is Result.ErrorOutOfDateKhr)
        {
           RecreateSwapChain();
           // Retry once after recreation
           acquireResult = _khrSwapchain.AcquireNextImage(
               _application.Device,
               _swapchain,
               ulong.MaxValue,
               _imageAvailableSemaphores[frame],
               default,
               ref imageIndex);
        }

        // VK_SUBOPTIMAL_KHR is still usable — just log and continue
        if (acquireResult is not Result.Success and not Result.SuboptimalKhr)
            acquireResult.EnsureSuccessed();

        _currentImageIndex = imageIndex;
        _currentFrameIndex = frame;
        var image = _swapChainImages[imageIndex];

        Debug.Assert(image.Handle is not 0);

        // 3. Transition image layout to ColorAttachmentOptimal for Skia rendering.
        //    Uses per-image layout tracking: first use is Undefined → ColorAttachmentOptimal,
        //    subsequent uses are PresentSrcKHR → ColorAttachmentOptimal.
        var oldLayout = _imageLayouts[imageIndex];
        RecordLayoutTransition(image, oldLayout, ImageLayout.ColorAttachmentOptimal);
        _imageLayouts[imageIndex] = ImageLayout.ColorAttachmentOptimal;

        var cmdBuffer = _commandBuffer;
        var waitSemaphore = _imageAvailableSemaphores[frame];
        PipelineStageFlags waitStage = PipelineStageFlags.ColorAttachmentOutputBit;
        SubmitInfo submitInfo = new()
        {
            SType = StructureType.SubmitInfo,
            WaitSemaphoreCount = 1,
            PWaitSemaphores = &waitSemaphore,
            PWaitDstStageMask = &waitStage,
            CommandBufferCount = 1,
            PCommandBuffers = &cmdBuffer,
        };
        _application.Vk.QueueSubmit(_application.GraphicsQueue, 1, ref submitInfo, default)
            .EnsureSuccessed();

        if (_surfaces[imageIndex] is { Handle: not 0 } surface)
            return surface;

        GRVkAlloc alloc = new();
        GRVkImageInfo imageInfo = new()
        {
            Image = image.Handle,
            ImageTiling = (uint)ImageTiling.Optimal,
            ImageLayout = (uint)ImageLayout.ColorAttachmentOptimal,
            Format = (uint)_swapChainImageFormat,
            ImageUsageFlags = (uint)_swapchainUsageFlags,
            SampleCount = 1,
            LevelCount = 1,
            CurrentQueueFamily = _application.Indices.GraphicsFamily!.Value,
            SharingMode = (uint)(_application.Indices.GraphicsFamily != _application.Indices.PresentFamily
                ? SharingMode.Concurrent
                : SharingMode.Exclusive),
            Alloc = alloc,
        };

        GRBackendRenderTarget backendRenderTarget = new(
            (int)_swapChainExtent.Width, (int)_swapChainExtent.Height, imageInfo);
        Debug.Assert(backendRenderTarget.IsValid);

        _surfaces[imageIndex] = surface = SKSurface.Create(
            _application.GRContext,
            backendRenderTarget,
            GRSurfaceOrigin.TopLeft,
            SKColorType.Bgra8888);

        Debug.Assert(surface is { Handle: not 0 });
        Debug.Assert(!_application.GRContext.IsAbandoned);

        return surface;
    }

    /// <summary>
    /// Complete the frame: flush Skia GPU commands, transition layout back to
    /// PresentSrcKHR, signal fence + renderFinished semaphore, and present.
    /// No CPU-side GPU wait — pipeline depth is controlled by
    /// <see cref="GetCurrent"/> waiting on fences from <c>MAX_FRAMES_IN_FLIGHT</c> ago.
    /// </summary>
    public unsafe void End()
    {
        Debug.Assert(_swapChainImages is not null);
        Debug.Assert(_imageLayouts is not null);

        // 1. Flush all pending Skia GPU work (submits to graphics queue)
        _application.GRContext.Flush();

        // 2. Record layout transition into command buffer (does NOT submit yet)
        var frame = _currentFrameIndex;
        var imageIndex = _currentImageIndex;
        RecordLayoutTransition(
            _swapChainImages[imageIndex],
            ImageLayout.ColorAttachmentOptimal,
            ImageLayout.PresentSrcKhr);

        // 3. Submit layout transition, signal fence + renderFinished semaphore.
        //    GPU queue order: [Skia rendering] → [layout transition] → fence + semaphore
        var commandBuffer = _commandBuffer;
        var fence = _inFlightFences[frame];
        var signalSemaphore = _renderFinishedSemaphores[frame];
        SubmitInfo submitInfo = new()
        {
            SType = StructureType.SubmitInfo,
            CommandBufferCount = 1,
            PCommandBuffers = &commandBuffer,
            SignalSemaphoreCount = 1,
            PSignalSemaphores = &signalSemaphore,
        };
        _application.Vk.QueueSubmit(_application.GraphicsQueue, 1, ref submitInfo, fence)
            .EnsureSuccessed();

        // 4. Track layout has changed to PresentSrcKHR
        _imageLayouts[imageIndex] = ImageLayout.PresentSrcKhr;

        // 5. Present — presentation engine GPU-waits on renderFinished semaphore,
        //    no CPU-side wait needed here.
        var swapchain = _swapchain;
        PresentInfoKHR presentInfo = new()
        {
            SType = StructureType.PresentInfoKhr,
            WaitSemaphoreCount = 1,
            PWaitSemaphores = &signalSemaphore,
            SwapchainCount = 1,
            PSwapchains = &swapchain,
            PImageIndices = &imageIndex,
        };

        _khrSwapchain!.QueuePresent(_application.PresentQueue, ref presentInfo)
            .EnsureSuccessed();
    }

    /// <summary>
    /// Record a pipeline barrier into <see cref="_commandBuffer"/> to transition
    /// an image between two layouts. Does NOT submit — caller must submit.
    /// </summary>
    private unsafe void RecordLayoutTransition(Image image, ImageLayout oldLayout, ImageLayout newLayout)
    {
        _application.Vk.ResetCommandBuffer(_commandBuffer, CommandBufferResetFlags.None);

        CommandBufferBeginInfo beginInfo = new()
        {
            SType = StructureType.CommandBufferBeginInfo,
            Flags = CommandBufferUsageFlags.OneTimeSubmitBit,
        };

        _application.Vk.BeginCommandBuffer(_commandBuffer, ref beginInfo);

        ImageMemoryBarrier barrier = new()
        {
            SType = StructureType.ImageMemoryBarrier,
            OldLayout = oldLayout,
            NewLayout = newLayout,
            SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
            DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
            Image = image,
            SubresourceRange = new ImageSubresourceRange
            {
                AspectMask = ImageAspectFlags.ColorBit,
                BaseMipLevel = 0,
                LevelCount = 1,
                BaseArrayLayer = 0,
                LayerCount = 1,
            },
        };

        PipelineStageFlags sourceStage;
        PipelineStageFlags destinationStage;

        if (oldLayout == ImageLayout.Undefined && newLayout == ImageLayout.ColorAttachmentOptimal)
        {
            barrier.SrcAccessMask = 0;
            barrier.DstAccessMask = AccessFlags.ColorAttachmentWriteBit;
            sourceStage = PipelineStageFlags.TopOfPipeBit;
            destinationStage = PipelineStageFlags.ColorAttachmentOutputBit;
        }
        else if (oldLayout == ImageLayout.PresentSrcKhr && newLayout == ImageLayout.ColorAttachmentOptimal)
        {
            barrier.SrcAccessMask = AccessFlags.MemoryReadBit;
            barrier.DstAccessMask = AccessFlags.ColorAttachmentWriteBit;
            sourceStage = PipelineStageFlags.BottomOfPipeBit;
            destinationStage = PipelineStageFlags.ColorAttachmentOutputBit;
        }
        else if (oldLayout == ImageLayout.ColorAttachmentOptimal && newLayout == ImageLayout.PresentSrcKhr)
        {
            barrier.SrcAccessMask = AccessFlags.ColorAttachmentWriteBit;
            barrier.DstAccessMask = 0;
            sourceStage = PipelineStageFlags.ColorAttachmentOutputBit;
            destinationStage = PipelineStageFlags.BottomOfPipeBit;
        }
        else
        {
            throw new InvalidOperationException(
                $"Unsupported layout transition: {oldLayout} → {newLayout}");
        }

        _application.Vk.CmdPipelineBarrier(
            _commandBuffer,
            sourceStage,
            destinationStage,
            DependencyFlags.None,
            0, null,
            0, null,
            1, ref barrier);

        _application.Vk.EndCommandBuffer(_commandBuffer);
    }

    private unsafe void DisposeCore()
    {
        if (_disposed)
            return;

        _disposed = true;

        _application.Vk.DeviceWaitIdle(_application.Device);

        CleanupSwapChain();

        // Destroy command buffer and pool
        if (_commandBuffer.Handle is not 0)
        {
            _application.Vk.FreeCommandBuffers(_application.Device, _commandPool, 1, ref _commandBuffer);
        }

        if (_commandPool.Handle is not 0)
        {
            _application.Vk.DestroyCommandPool(_application.Device, _commandPool, null);
        }

        // Destroy sync objects
        for (int i = 0; i < MAX_FRAMES_IN_FLIGHT; i++)
        {
            if (_renderFinishedSemaphores[i].Handle is not 0)
                _application.Vk.DestroySemaphore(_application.Device, _renderFinishedSemaphores[i], null);

            if (_imageAvailableSemaphores[i].Handle is not 0)
                _application.Vk.DestroySemaphore(_application.Device, _imageAvailableSemaphores[i], null);

            if (_inFlightFences[i].Handle is not 0)
                _application.Vk.DestroyFence(_application.Device, _inFlightFences[i], null);
        }
    }

    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }

    ~VulkanWindow()
    {
        DisposeCore();
    }

}