using Silk.NET.Vulkan;

namespace Shimakaze.UI.Rendering.Vulkan;

internal sealed class SwapChainSupportDetails
{
    public SurfaceCapabilitiesKHR Capabilities { get; set; }
    public IReadOnlyList<SurfaceFormatKHR> Formats { get; set; } = [];
    public IReadOnlyList<PresentModeKHR> PresentModes { get; set; } = [];
}
