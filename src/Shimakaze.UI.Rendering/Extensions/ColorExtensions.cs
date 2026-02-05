using System.Drawing;


using SkiaSharp;

namespace Shimakaze.UI.Rendering.Extensions;

public static class ColorExtensions
{
    public static SKColor ToSkia(this Color color)
        => new(color.R, color.G, color.B, color.A);
    public static Color ToDrawing(this SKColor color)
        => Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
}