using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class KeyboardKeyPressedEventArgs(IKeyboard keyboard, Key key, int scancode, bool isRepeat, int repeatCount) : KeyboardEventArgs(keyboard)
{
    public Key Key { get; } = key;

    public int Scancode { get; } = scancode;

    public bool IsRepeat { get; } = isRepeat;

    public int RepeatCount { get; } = repeatCount;
}