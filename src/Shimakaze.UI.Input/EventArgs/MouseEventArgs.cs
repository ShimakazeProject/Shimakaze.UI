using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public abstract class MouseEventArgs(IMouse mouse) : InputDeviceEventArgs(mouse)
{
    public IMouse Mouse { get; } = mouse;
}