using Silk.NET.Windowing;

namespace Shimakaze.UI;

public sealed class WindowStateChangedEventArgs(WindowState newState) : EventArgs
{
    public WindowState State { get; } = newState;
}