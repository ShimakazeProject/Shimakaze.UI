using System.Drawing;


using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public static class ColorExtensions
{
    /// <summary>
    /// 将 System.Drawing.Color 转换为 SKColor。
    /// </summary>
    public static SKColor ToSKColor(this Color color)
    {
        return new SKColor(
            color.R,
            color.G,
            color.B,
            color.A
        );
    }

    /// <summary>
    /// 将 SKColor 转换为 System.Drawing.Color。
    /// </summary>
    public static Color ToDrawingColor(this SKColor color)
    {
        return Color.FromArgb(
            color.Alpha,
            color.Red,
            color.Green,
            color.Blue
        );
    }
}