using Shimakaze.UI.Core;

using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public abstract class Renderer : IRenderer
{
    public SKSurface CreateSurface(INativeWindow window)
        => CreateSurface(window.Native);

    protected abstract SKSurface CreateSurface(IWindow window);
}