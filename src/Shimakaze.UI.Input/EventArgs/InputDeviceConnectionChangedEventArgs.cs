using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class InputDeviceConnectionChangedEventArgs(IInputDevice device, bool connected) : InputDeviceEventArgs(device)
{
    public bool Connected { get; } = connected;
}