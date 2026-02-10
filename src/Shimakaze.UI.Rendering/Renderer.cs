using System.Drawing;
using System.Numerics;

using Shimakaze.UI.Rendering.Extensions;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public abstract class Renderer(SKSurface surface) : IDisposable
{
    public SKSurface Surface { get; } = surface;

    public SKCanvas Canvas => Surface.Canvas;

    /// <summary>
    /// 获取绝对坐标
    /// </summary>
    /// <remarks>
    /// 相对坐标是旋转无关的坐标，绝对坐标是旋转后的坐标。<br />
    /// 此方法是给内部使用的 用于计算坐标位置，便于在 Canvas 上绘制
    /// </remarks>
    /// <param name="point">相对坐标</param>
    /// <returns>绝对坐标</returns>
    public virtual Vector2 FixPosition(Vector2 point) => point;
    public PointF FixPosition(PointF point) => new(FixPosition((Vector2)point));
    public PointF FixPosition(float x, float y) => FixPosition(new PointF(x, y));
    public RectangleF FixPosition(RectangleF rect)
    {
        var v1 = FixPosition(rect.Location);
        var v2 = FixPosition(rect.Right, rect.Bottom);
        return RectangleF.FromLTRB(v1.X, v1.Y, v2.X, v2.Y);
    }

    public Renderer Clear(Color color)
    {
        Canvas.Clear(color.ToSkia());
        return this;
    }

    #region Image
    public Renderer DrawImage(SKImage image, PointF point, SKPaint? paint = default)
    {
        point = FixPosition(point);
        Canvas.DrawImage(image, point.ToSkia(), paint);
        return this;
    }

    public Renderer DrawImage(SKImage image, SizeF source, RectangleF dest, SKPaint? paint = default)
        => DrawImage(image, new RectangleF(PointF.Empty, source), dest, paint);

    public Renderer DrawImage(SKImage image, RectangleF dest, SKPaint? paint = default)
    {
        dest = FixPosition(dest);
        Canvas.DrawImage(image, dest.ToSkia(), paint);
        return this;
    }

    public Renderer DrawImage(SKImage image, RectangleF source, RectangleF dest, SKPaint? paint = default)
    {
        dest = FixPosition(dest);
        Canvas.DrawImage(image, source.ToSkia(), dest.ToSkia(), paint);
        return this;
    }
    #endregion

    #region Bitmap
    public Renderer DrawBitmap(SKBitmap bitmap, PointF point, SKPaint? paint = default)
    {
        point = FixPosition(point);
        Canvas.DrawBitmap(bitmap, point.ToSkia(), paint);
        return this;
    }
    public Renderer DrawBitmap(SKBitmap bitmap, float x, float y, SKPaint? paint = default)
         => DrawBitmap(bitmap, new PointF(x, y), paint);

    public Renderer DrawBitmap(SKBitmap bitmap, RectangleF dest, SKPaint? paint = default)
    {
        dest = FixPosition(dest);
        Canvas.DrawBitmap(bitmap, dest.ToSkia(), paint);
        return this;
    }

    public Renderer DrawBitmap(SKBitmap bitmap, float x, float y, float width, float height, SKPaint? paint = default)
        => DrawBitmap(bitmap, new RectangleF(x, y, width, height), paint);

    public Renderer DrawBitmap(SKBitmap bitmap, RectangleF source, RectangleF dest, SKPaint? paint = default)
    {
        dest = FixPosition(dest);
        Canvas.DrawBitmap(bitmap, source.ToSkia(), dest.ToSkia(), paint);
        return this;
    }
    #endregion

    #region Rotate
    public RotatedRenderer RotateDegrees(float degrees)
        => RotateRadians(RotatedRenderer.GetRadians(degrees));
    public RotatedRenderer RotateDegrees(PointF origin, float degrees)
        => RotateRadians(origin, RotatedRenderer.GetRadians(degrees));
    public RotatedRenderer RotateDegrees(float x, float y, float degrees)
        => RotateRadians(new(x, y), RotatedRenderer.GetRadians(degrees));
    public RotatedRenderer RotateRadians(float radians)
        => RotateRadians(PointF.Empty, radians);
    public RotatedRenderer RotateRadians(PointF origin, float radians)
        => new(this, origin, radians);
    public RotatedRenderer RotateRadians(float x, float y, float radians)
        => RotateRadians(new(x, y), radians);
    #endregion

    public ClippedRenderer ClipRect(RectangleF rect) => new(this, rect);

    public abstract void Dispose();
}