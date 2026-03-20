using System.Drawing;

using Shimakaze.UI.Fonts;
using Shimakaze.UI.Rendering;

using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace Shimakaze.UI;

public static class RendererExtensions
{
    private static readonly Lazy<SKPaint> DefaultPaint = new(() => new()
    {
        Color = SKColors.Black,
        IsAntialias = true,
        IsDither = false,
    });

    public static Renderer DrawText(this Renderer renderer, string text, RectangleF rect, TextAlign textAlign = TextAlign.Left, Font? font = default, SKPaint? paint = default)
    {
        using var clip = renderer.ClipRect(rect);

        var size = FontManager.Measure(text, font);
        paint ??= DefaultPaint.Value;
        var point = clip.FixPosition(rect.X, rect.Y + size.Height);
        clip.Canvas.DrawShapedText(
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
}