using SkiaSharp;

namespace Shimakaze.UI.Fonts;

public static class FontManager
{
    private static readonly Lazy<SKFont> DefaultFont = new(() => new(SKTypeface.Default));
    private static readonly Dictionary<Font, SKFontStyle> Fontstyles = [];
    private static readonly Dictionary<Font, SKTypeface> Typefaces = [];
    private static readonly Dictionary<Font, SKFont> Fonts = [];

    public static SKFont GetFont(Font? font)
    {
        if (font is null)
            return DefaultFont.Value;

        if (!Fontstyles.TryGetValue(font, out var fontstyle))
        {
            Fontstyles[font] = fontstyle = new(font.Weight, font.Width, font.Slant switch
            {
                FontStyleSlant.Upright => SKFontStyleSlant.Upright,
                FontStyleSlant.Italic => SKFontStyleSlant.Italic,
                FontStyleSlant.Oblique => SKFontStyleSlant.Oblique,
                _ => throw new NotSupportedException(),
            });

            font.Disposed += () => Fontstyles.Remove(font);
        }

        if (!Typefaces.TryGetValue(font, out var typeface))
        {
            Typefaces[font] = typeface = SKTypeface.FromFamilyName(font.FamilyName, fontstyle);

            font.Disposed += () => Typefaces.Remove(font);
        }

        if (!Fonts.TryGetValue(font, out var skFont))
        {
            Fonts[font] = skFont = new(typeface, font.Size, font.ScaleX, font.SkewX);

            font.Disposed += () => Fonts.Remove(font);
        }

        return skFont;
    }
}