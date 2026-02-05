using Shimakaze.UI.Core;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface IRendererProvider
{
    SKSurface GetSurface(INativeWindow window);

    BaseRenderer GetRenderer(INativeWindow window);
}