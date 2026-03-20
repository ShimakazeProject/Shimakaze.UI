namespace Shimakaze.UI;

public sealed class UpdateEventArgs(double deltaTime) : EventArgs
{
    public double DeltaTime { get; } = deltaTime;
}