using Shimakaze.UI.Core;

using Silk.NET.Windowing;

namespace Shimakaze.UI.Rendering.OpenGL;

public sealed class OpenGLProvider : IPlatformWindowOptionsProvider, ISurfaceProviderFactory
{
    public ISurfaceProvider Create(IWindow window) => new OpenGLSurfaceProvider(window);

    public WindowOptions CreateOptions() => WindowOptions.Default;
}