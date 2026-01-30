using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public abstract class GamepadEventArgs(IGamepad gamepad) : InputDeviceEventArgs(gamepad)
{
    public IGamepad Gamepad { get; } = gamepad;
}