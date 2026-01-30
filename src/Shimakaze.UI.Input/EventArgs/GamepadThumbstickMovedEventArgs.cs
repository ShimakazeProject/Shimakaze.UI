using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class GamepadThumbstickMovedEventArgs(IGamepad gamepad, Thumbstick thumbstick) : GamepadEventArgs(gamepad)
{
    public Thumbstick Thumbstick { get; } = thumbstick;
}