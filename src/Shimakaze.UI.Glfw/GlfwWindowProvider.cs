using Shimakaze.UI.Core;

using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;

namespace Shimakaze.UI.Glfw;

public sealed class GlfwWindowProvider : IWindowProvider
{
    public IWindow CreateWindow(WindowOptions options)
    {
        GlfwWindowing.RegisterPlatform();

        return Silk.NET.Windowing.Window.Create(options);
    }
}