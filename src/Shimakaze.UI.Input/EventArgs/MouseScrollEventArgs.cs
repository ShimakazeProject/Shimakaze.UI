using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class MouseScrollEventArgs(IMouse mouse, ScrollWheel wheel) : MouseEventArgs(mouse)
{
    public ScrollWheel Wheel { get; } = wheel;
}