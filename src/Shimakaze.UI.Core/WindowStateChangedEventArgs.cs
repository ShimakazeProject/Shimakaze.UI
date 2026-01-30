using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public sealed class WindowStateChangedEventArgs(WindowState newState) : EventArgs
{
    public WindowState State { get; } = newState;
}