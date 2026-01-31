using Shimakaze.UI.Core;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface IRenderer
{
    SKSurface GetSurface(INativeWindow window);
}