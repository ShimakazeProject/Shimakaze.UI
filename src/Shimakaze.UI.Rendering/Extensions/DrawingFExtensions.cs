using System.Drawing;


using SkiaSharp;

namespace Shimakaze.UI.Rendering.Extensions;

public static class DrawingFExtensions
{
    public static SKPoint ToSkia(this PointF point)
        => new(point.X, point.Y);
    public static PointF ToDrawing(this SKPoint point)
        => new(point.X, point.Y);

    public static SKSize ToSkia(this SizeF size)
        => new(size.Width, size.Height);
    public static SizeF ToDrawing(this SKSize size)
        => new(size.Width, size.Height);

    public static SKRect ToSkia(this RectangleF rect)
        => new(rect.Left, rect.Top, rect.Right, rect.Bottom);
    public static RectangleF ToDrawing(this SKRect rect)
        => new(rect.Left, rect.Top, rect.Width, rect.Height);
}