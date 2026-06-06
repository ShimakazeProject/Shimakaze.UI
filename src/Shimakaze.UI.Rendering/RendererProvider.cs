using System.Collections.Concurrent;

using Shimakaze.UI.Core;

namespace Shimakaze.UI.Rendering;

public sealed class RendererProvider(ISurfaceProviderFactory rendererContextProvider)
{
    private readonly ConcurrentDictionary<PlatformWindow, ISurfaceProvider> _cache = [];
    public BaseRenderer GetRenderer(PlatformWindow window) => new(_cache.GetOrAdd(
        window,
        window =>
        {
            window.Closed += Window_Closed;
            return rendererContextProvider.Create(window.Native);
        }));

    private async void Window_Closed(PlatformWindow sender, EventArgs eventArgs)
    {
        if (_cache.TryRemove(sender, out var provider))
        {
            provider.Dispose();
            return;
        }

        await Task.Delay(500);
        Window_Closed(sender, eventArgs);
    }
}
