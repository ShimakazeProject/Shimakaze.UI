using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public class MouseButtonEventArgs(IMouse mouse, MouseButton button) : MouseEventArgs(mouse)
{
    public MouseButton Button { get; } = button;
}