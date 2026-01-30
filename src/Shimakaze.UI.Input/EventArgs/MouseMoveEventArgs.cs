using System.Numerics;

using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class MouseMoveEventArgs(IMouse mouse, Vector2 position) : MouseEventArgs(mouse)
{
    public Vector2 Position { get; } = position;
}