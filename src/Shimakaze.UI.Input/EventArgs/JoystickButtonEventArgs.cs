using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class JoystickButtonEventArgs(IJoystick joystick, Button button) : JoystickEventArgs(joystick)
{
    public Button Button { get; } = button;
}