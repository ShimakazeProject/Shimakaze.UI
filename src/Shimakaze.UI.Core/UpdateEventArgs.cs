namespace Shimakaze.UI.Core;

public sealed class UpdateEventArgs(double deltaTime) : EventArgs
{
    public double DeltaTime { get; } = deltaTime;
}