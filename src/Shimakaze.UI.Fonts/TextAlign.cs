using SkiaSharp;

namespace Shimakaze.UI.Fonts;

public enum TextAlign
{
    Left,
    Center,
    Right,
}

public static class TextAlignExtensions
{
    public static SKTextAlign ToSkia(this TextAlign align) => align switch
    {
        TextAlign.Left => SKTextAlign.Left,
        TextAlign.Center => SKTextAlign.Center,
        TextAlign.Right => SKTextAlign.Right,
        _ => throw new NotSupportedException(),

    };

    public static TextAlign ToUI(this SKTextAlign align) => align switch
    {
        SKTextAlign.Left => TextAlign.Left,
        SKTextAlign.Center => TextAlign.Center,
        SKTextAlign.Right => TextAlign.Right,
        _ => throw new NotSupportedException(),
    };
}