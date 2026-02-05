using Shimakaze.UI.Core;

using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public abstract class RendererProvider : IRendererProvider
{
    public SKSurface GetSurface(INativeWindow window)
        => GetSurface(window.Native);

    protected abstract SKSurface GetSurface(IWindow window);
}