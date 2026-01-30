using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class GamepadTriggerMovedEventArgs(IGamepad gamepad, Trigger trigger) : GamepadEventArgs(gamepad)
{
    public Trigger Trigger { get; } = trigger;
}