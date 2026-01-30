using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public abstract class Renderer : IRenderer
{
    public SKSurface CreateSurface(Core.Window window)
    {
        return CreateSurface(window.Native);
    }

    protected abstract SKSurface CreateSurface(IWindow window);
}