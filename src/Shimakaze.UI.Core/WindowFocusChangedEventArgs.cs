namespace Shimakaze.UI;

public sealed class WindowFocusChangedEventArgs(bool focused) : EventArgs
{
    public bool Focused { get; } = focused;
}