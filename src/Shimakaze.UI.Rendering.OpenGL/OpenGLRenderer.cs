using System.Collections.Concurrent;

using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering.OpenGL;

public sealed class OpenGLRenderer : Renderer
{
    private readonly ConcurrentDictionary<IWindow, OpenGLRendererContext> _cache = [];
    private GRContext? _grContext;

    protected override SKSurface GetSurface(IWindow window)
    {
        var context = _cache.GetOrAdd(window, _ => new());
        if (context.ShouldRecreate(window))
            context.Create(window, ref _grContext);

        return context.Surface;
    }
}