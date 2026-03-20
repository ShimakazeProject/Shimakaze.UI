using System.Drawing;

using Shimakaze.UI;
using Shimakaze.UI.Fonts;
using Shimakaze.UI.Rendering;

using SkiaSharp;
using SkiaSharp.HarfBuzz;

static class RendererExtensions
{
    private static readonly Lazy<SKPaint> DefaultPaint = new(() => new()
    {
        Color = SKColors.White,
        IsAntialias = true,
        IsDither = false,
    });

    public static Renderer DrawText(this Renderer renderer, string text, PointF point, TextAlign textAlign = TextAlign.Left, Font? font = default, SKPaint? paint = default)
    {
        paint ??= DefaultPaint.Value;
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
