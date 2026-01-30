using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class JoystickAxisMovedEventArgs(IJoystick joystick, Axis axis) : JoystickEventArgs(joystick)
{
    public Axis Axis { get; } = axis;
}