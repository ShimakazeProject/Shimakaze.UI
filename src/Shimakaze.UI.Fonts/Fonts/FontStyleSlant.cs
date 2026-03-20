using SkiaSharp;

namespace Shimakaze.UI.Fonts;

public enum FontStyleSlant
{
    Upright = 0,
    Italic = 1,
    Oblique = 2,
}

public static class FontStyleSlantExtensions
{
    public static SKFontStyleSlant ToSkia(this FontStyleSlant slant) => slant switch
    {
        FontStyleSlant.Upright => SKFontStyleSlant.Upright,
        FontStyleSlant.Italic => SKFontStyleSlant.Italic,
        FontStyleSlant.Oblique => SKFontStyleSlant.Oblique,
        _ => throw new NotSupportedException(),
    };

    public static FontStyleSlant ToUI(this SKFontStyleSlant slant) => slant switch
    {
        SKFontStyleSlant.Upright => FontStyleSlant.Upright,
        SKFontStyleSlant.Italic => FontStyleSlant.Italic,
        SKFontStyleSlant.Oblique => FontStyleSlant.Oblique,
        _ => throw new NotSupportedException(),
    };
}