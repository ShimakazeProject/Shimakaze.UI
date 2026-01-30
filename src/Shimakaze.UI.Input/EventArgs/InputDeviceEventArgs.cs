using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public abstract class InputDeviceEventArgs(IInputDevice device) : System.EventArgs
{
    public IInputDevice Device { get; } = device;
}