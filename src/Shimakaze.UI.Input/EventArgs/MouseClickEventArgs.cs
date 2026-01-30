using System.Numerics;

using Silk.NET.Input;

namespace Shimakaze.UI.Input.EventArgs;

public sealed class MouseClickEventArgs(IMouse mouse, MouseButton button, Vector2 position) : MouseButtonEventArgs(mouse, button)
{
    public Vector2 Position { get; } = position;
}