using System.Drawing;

using Shimakaze.Foundation.Rendering;
using Shimakaze.UI.Fonts;

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
        using var clip = renderer.FixedClipRect(rect);

        var size = FontManager.Measure(text, font);
        paint ??= DefaultPaint.Value;
        var point = clip.FixPosition(0, size.Height);
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