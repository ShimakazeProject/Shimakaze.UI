using Shimakaze.UI.Core;

using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public abstract class RendererProvider : IRendererProvider
{
    public virtual BaseRenderer GetRenderer(PlatformWindow window)
        => new(GetSurface(window.Native));

    public virtual SKSurface GetSurface(PlatformWindow window)
        => GetSurface(window.Native);

    protected abstract SKSurface GetSurface(IWindow window);
}