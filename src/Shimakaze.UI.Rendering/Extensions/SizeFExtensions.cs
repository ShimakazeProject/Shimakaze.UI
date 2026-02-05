using System.Drawing;
using System.Numerics;

namespace Shimakaze.UI.Rendering.Extensions;

public static class SizeFExtensions
{
    public static SizeF ToDrawing(this Vector2 vector)
        => new(vector.X, vector.Y);
    public static Vector2 ToVector2(this SizeF point)
        => new(point.Width, point.Height);
}