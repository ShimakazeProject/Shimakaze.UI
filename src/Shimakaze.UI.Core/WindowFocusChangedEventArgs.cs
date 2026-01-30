namespace Shimakaze.UI.Core;

public sealed class WindowFocusChangedEventArgs(bool focused) : EventArgs
{
    public bool Focused { get; } = focused;
}