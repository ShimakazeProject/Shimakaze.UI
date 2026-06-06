using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface ISurfaceProvider
{
    SKSurface Begin();

    void End();
}