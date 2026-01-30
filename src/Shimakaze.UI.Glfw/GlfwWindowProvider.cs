using Shimakaze.UI.Core;
using Shimakaze.UI.Input;

using Silk.NET.Input;
using Silk.NET.Input.Glfw;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;

namespace Shimakaze.UI.Glfw;

public sealed class GlfwProvider : IWindowProvider, IInputContextProvider
{
    public IInputContext CreateInputContext(IWindow window)
    {
        GlfwInput.RegisterPlatform();

        return window.CreateInput();
    }

    public IWindow CreateWindow(WindowOptions options)
    {
        GlfwWindowing.RegisterPlatform();

        return Silk.NET.Windowing.Window.Create(options);
    }
}