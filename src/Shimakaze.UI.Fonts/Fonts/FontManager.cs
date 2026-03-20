using System.Drawing;

using Shimakaze.UI.Rendering.Extensions;

using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace Shimakaze.UI.Fonts;

public static class FontManager
{
    private static readonly Lazy<SKFont> DefaultFont = new(() => new(SKTypeface.Default));
    private static readonly Dictionary<Font, SKFontStyle> Fontstyles = [];
    private static readonly Dictionary<Font, SKTypeface> Typefaces = [];
    private static readonly Dictionary<Font, SKFont> Fonts = [];
    private static readonly Dictionary<Font, SKShaper> Shapers = [];
    private static SKShaper? s_defaultShapers;

    public static SKFontStyle GetFontStyle(Font? font = default)
    {
        if (font is null)
            return SKFontStyle.Normal;

        if (!Fontstyles.TryGetValue(font, out var fontstyle))
        {
            Fontstyles[font] = fontstyle = new(font.Weight, font.Width, font.Slant.ToSkia());

            font.Disposed += () => Fontstyles.Remove(font);
        }

        return fontstyle;
    }

    public static SKTypeface GetTypeface(Font? font = default, SKFontStyle? fontStyle = default)
    {
        if (font is null)
            return SKTypeface.Default;

        fontStyle ??= GetFontStyle(font);

        if (!Typefaces.TryGetValue(font, out var typeface))
        {
            Typefaces[font] = typeface = font switch
            {
                FamilyNameFont familyNameFont => SKTypeface.FromFamilyName(familyNameFont.FamilyName, fontStyle),
                FilePathFont filePathFont => SKTypeface.FromFile(filePathFont.FilePath, filePathFont.Index),
                StreamFont streamFont => SKTypeface.FromStream(streamFont.Stream, streamFont.Index),
                _ => throw new NotSupportedException(),
            };
            font.Disposed += () => Typefaces.Remove(font);
        }

        return typeface;
    }
    public static SKShaper GetShaper(Font? font = default, SKTypeface? typeface = default)
    {
        typeface ??= GetTypeface(font);
        if (font is null)
            return s_defaultShapers ??= new(typeface);

        if (!Shapers.TryGetValue(font, out var shaper))
        {
            Shapers[font] = shaper = new(typeface);

            font.Disposed += () => Shapers.Remove(font);
        }

        return shaper;
    }
    public static SKFont GetFont(Font? font = default, SKTypeface? typeface = default, SKFontStyle? fontStyle = default)
    {
        if (font is null)
            return DefaultFont.Value;

        fontStyle ??= GetFontStyle(font);
        typeface ??= GetTypeface(font, fontStyle);

        if (!Fonts.TryGetValue(font, out var skFont))
        {
            Fonts[font] = skFont = new(typeface, font.Size, font.ScaleX, font.SkewX);

            font.Disposed += () => Fonts.Remove(font);
        }

        return skFont;
    }

    public static RectangleF Measure(string text, Font? font = null)
    {
        var skfont = FontManager.GetFont(font);
        skfont.MeasureText(text, out var bounds);
        return bounds.ToDrawing();
    }
}