using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class KeyboardKeyEventArgs(IKeyboard keyboard, Key key, int scancode) : KeyboardEventArgs(keyboard)
{
    public Key Key { get; } = key;
    public int Scancode { get; } = scancode;
}