using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class GamepadButtonEventArgs(IGamepad gamepad, Button button) : GamepadEventArgs(gamepad)
{
    public Button Button { get; } = button;
}