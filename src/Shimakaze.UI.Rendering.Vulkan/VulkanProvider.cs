using Shimakaze.UI.Core;

using Silk.NET.Windowing;

namespace Shimakaze.UI.Rendering.Vulkan;

public sealed class VulkanProvider : IPlatformWindowOptionsProvider, ISurfaceProviderFactory
{
    public ISurfaceProvider Create(IWindow window) => new VulkanSurfaceProvider(window);

    public WindowOptions CreateOptions() => WindowOptions.DefaultVulkan;
}