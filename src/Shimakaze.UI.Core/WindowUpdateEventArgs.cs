namespace Shimakaze.UI.Core;

public sealed class WindowUpdateEventArgs(double deltaTime) : EventArgs
{
    public double DeltaTime { get; } = deltaTime;
}