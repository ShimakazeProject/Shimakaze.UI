using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class JoystickHatMovedEventArgs(IJoystick joystick, Hat hat) : JoystickEventArgs(joystick)
{
    public Hat Hat { get; } = hat;
}