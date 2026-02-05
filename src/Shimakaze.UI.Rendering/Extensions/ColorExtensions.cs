using System.Diagnostics.CodeAnalysis;
using System.Drawing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering.Extensions;

public static class ColorExtensions
{
    public static SKColor ToSkia(this Color color)
        => new(color.R, color.G, color.B, color.A);
    public static Color ToDrawing(this SKColor color)
        => Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);

    extension(Color @this)
    {
        public static Color FromHsl(float h, float s, float v)
            => SKColor.FromHsl(h, s, v).ToDrawing();
        public static Color FromHsv(float h, float s, float v)
            => SKColor.FromHsv(h, s, v).ToDrawing();

        public string ToHexString()
            => $"#{@this.ToArgb():X8}";

        public static Color ParseHex(string hex)
            => SKColor.Parse(hex).ToDrawing();
        public static bool TryParseHex(string hex, [NotNullWhen(true)] out Color color)
        {
            if (SKColor.TryParse(hex, out var sk))
            {
                color = sk.ToDrawing();
                return true;
            }

            color = default;
            return false;
        }
    }
}