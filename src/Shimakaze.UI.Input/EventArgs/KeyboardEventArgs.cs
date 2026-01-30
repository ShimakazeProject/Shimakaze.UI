using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public abstract class KeyboardEventArgs(IKeyboard keyboard) : InputDeviceEventArgs(keyboard)
{
    public IKeyboard Keyboard { get; } = keyboard;
}