using System.Drawing;
using System.Numerics;

namespace Shimakaze.UI.Rendering.Extensions;

public static class PointFExtensions
{
    public static PointF ToDrawing(this Vector2 vector)
        => new(vector.X, vector.Y);
    public static Vector2 ToVector2(this PointF point)
        => new(point.X, point.Y);
}