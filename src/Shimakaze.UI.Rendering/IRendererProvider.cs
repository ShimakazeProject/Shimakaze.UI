using Shimakaze.UI.Core;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface IRendererProvider
{
    SKSurface GetSurface(PlatformWindow window);

    BaseRenderer GetRenderer(PlatformWindow window);
}