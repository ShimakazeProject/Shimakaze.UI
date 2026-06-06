using Silk.NET.Windowing;

namespace Shimakaze.UI.Rendering;

public interface ISurfaceProviderFactory
{
    ISurfaceProvider Create(IWindow window);
}