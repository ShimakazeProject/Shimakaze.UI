using Shimakaze.UI.Core;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface IRenderer
{
    SKSurface CreateSurface(INativeWindow window);
}
