using System.Collections.Concurrent;

using Shimakaze.UI.Core;

using Silk.NET.Windowing;

namespace Shimakaze.UI.Rendering;

public sealed class RendererProvider(ISurfaceProviderFactory rendererContextProvider)
{
    private readonly ConcurrentDictionary<IWindow, ISurfaceProvider> _cache = [];
    public BaseRenderer GetRenderer(PlatformWindow window) => new(_cache.GetOrAdd(window.Native, rendererContextProvider.Create));
}
