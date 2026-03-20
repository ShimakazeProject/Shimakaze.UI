using Silk.NET.Maths;

namespace Shimakaze.UI;

public sealed class WindowResizeEventArgs(Vector2D<int> newSize) : EventArgs
{
    public Vector2D<int> NewSize { get; } = newSize;
}