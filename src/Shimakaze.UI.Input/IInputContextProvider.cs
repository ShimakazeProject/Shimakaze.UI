using Shimakaze.UI.Core;

using Silk.NET.Input;
using Silk.NET.Windowing;

namespace Shimakaze.UI.Input;

public interface IInputContextProvider
{
    IInputContext CreateInputContext(IWindow window);

    InputManager CreateInputManager(Core.PlatformWindow window)
        => CoreExtensions.GetInputManager(this, window);
}