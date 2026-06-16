using Shimakaze.Foundation.Rendering;

namespace Shimakaze.UI;

public sealed class RenderEventArgs(Renderer renderer, double deltaTime) : EventArgs
{
    public Renderer Renderer { get; } = renderer;
    public double DeltaTime { get; } = deltaTime;
}