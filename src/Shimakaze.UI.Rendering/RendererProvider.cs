using System.Collections.Concurrent;

using Shimakaze.UI.Core;

using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public sealed class RendererProvider(ISurfaceProviderFactory rendererContextProvider)
{
    private readonly ConcurrentDictionary<IWindow, ISurfaceProvider> _cache = [];
    public BaseRenderer GetRenderer(PlatformWindow window)
        => new(GetSurface(window.Native));

    public SKSurface GetSurface(PlatformWindow window)
        => GetSurface(window.Native);

    private SKSurface GetSurface(IWindow window)
    {
        var context = _cache.GetOrAdd(window, rendererContextProvider.Create);
        if (context.IsInvalid())
            context.EnsureCreated();

        return context.Surface;
    }
}
