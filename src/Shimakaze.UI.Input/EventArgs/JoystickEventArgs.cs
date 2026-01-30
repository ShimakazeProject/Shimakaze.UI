using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public abstract class JoystickEventArgs(IJoystick joystick) : InputDeviceEventArgs(joystick)
{
    public IJoystick Joystick { get; } = joystick;
}