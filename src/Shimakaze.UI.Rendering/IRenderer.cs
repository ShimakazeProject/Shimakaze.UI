using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface IRenderer
{
    SKSurface CreateSurface(Core.Window window);
}
