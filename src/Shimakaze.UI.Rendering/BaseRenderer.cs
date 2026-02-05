using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public sealed class BaseRenderer(SKSurface surface) : Renderer(surface)
{
    public override void Dispose()
    {
        Canvas.Flush();
        Surface.Flush();
    }
}