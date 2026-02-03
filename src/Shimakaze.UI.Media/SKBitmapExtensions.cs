using Silk.NET.Core;

using SkiaSharp;

namespace Shimakaze.UI.Media;

public static class SKBitmapExtensions
{
    public static RawImage AsRawImage(this SKBitmap bitmap)
    {
        if (bitmap.ColorType is SKColorType.Rgba8888)
            return new(bitmap.Width, bitmap.Height, bitmap.Bytes);

        using SKBitmap tmpbmp = new(bitmap.Width, bitmap.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using SKCanvas canvas = new(tmpbmp);
        canvas.DrawBitmap(bitmap, 0, 0);
        return new(tmpbmp.Width, tmpbmp.Height, tmpbmp.Bytes);
    }
}