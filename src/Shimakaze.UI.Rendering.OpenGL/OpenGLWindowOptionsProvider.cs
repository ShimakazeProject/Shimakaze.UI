using Shimakaze.UI.Core;

using Silk.NET.Windowing;

namespace Shimakaze.UI.Rendering.OpenGL;

public sealed class OpenGLWindowOptionsProvider : PlatformWindowOptionsProvider
{
    public WindowOptions CreateOptions()
    {
        return WindowOptions.Default;
    }
}