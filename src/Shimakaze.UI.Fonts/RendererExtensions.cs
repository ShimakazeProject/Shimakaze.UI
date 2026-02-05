using System.Drawing;

using Shimakaze.UI.Rendering;

using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace Shimakaze.UI.Fonts;

public static class RendererExtensions
{
    private static readonly Lazy<SKPaint> s_defaultPaint = new(() => new()
    {
        Color = SKColors.White,
        IsAntialias = true,
        IsDither = false,
    });

    public static Renderer DrawText(this Renderer renderer, string text, PointF point, TextAlign textAlign = TextAlign.Left, Font? font = default, SKPaint? paint = default)
    {
        paint ??= s_defaultPaint.Value;
        point = renderer.FixPosition(point);
        renderer.Canvas.DrawShapedText(
            FontManager.GetShaper(font),
            text,
            point.X,
            point.Y,
            textAlign.ToSkia(),
            FontManager.GetFont(font),
            paint
        );

        return renderer;
    }
    public static Renderer DrawText(this Renderer renderer, string text, float x, float y, TextAlign textAlign = TextAlign.Left, Font? font = default, SKPaint? paint = default)
        => renderer.DrawText(text, new PointF(x, y), textAlign, font, paint);
}