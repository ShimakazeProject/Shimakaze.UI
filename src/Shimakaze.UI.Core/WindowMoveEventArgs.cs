using Silk.NET.Maths;

namespace Shimakaze.UI;

public sealed class WindowMoveEventArgs(Vector2D<int> newPosition) : EventArgs
{
    public Vector2D<int> NewPosition { get; } = newPosition;
}