using System.Diagnostics.CodeAnalysis;

namespace Shimakaze.UI.Rendering.Vulkan;

internal sealed class QueueFamilyIndices
{
    public uint? GraphicsFamily { get; set; }
    [MemberNotNullWhen(true, nameof(GraphicsFamily), nameof(PresentFamily))]
    public bool IsComplete => GraphicsFamily.HasValue && PresentFamily.HasValue;

    public uint? PresentFamily { get; set; }
}
