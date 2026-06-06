using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface ISurfaceProvider : IDisposable
{
    SKSurface Begin();

    void End();
}