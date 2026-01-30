using Shimakaze.UI.Input.Cursors;
using Shimakaze.UI.Media.Ico;

using Silk.NET.Core;
using Silk.NET.Maths;

using SkiaSharp;

namespace Shimakaze.UI.Media.Cursor;

public static class CurDecoder
{
    public static Input.Cursors.Cursor Decode(Stream stream)
    {
        var (hotspot, bitmap) = DecodeCore(stream);
        return Input.Cursors.Cursor.Custom(new CursorFrame(hotspot, bitmap));
    }

    internal static (Vector2D<int> Hotspot, RawImage Bitmap) DecodeCore(Stream stream)
    {
        var (bitmap, entry, _, _) = IcoDecoder.DecodeCore(stream).MaxBy(i => i switch
        {
            { IsPng: true } => int.MaxValue,
            { BitmapInfo: not null } => i.BitmapInfo.Value.Width * i.BitmapInfo.Value.Height * (i.BitmapInfo.Value.BitCount / 8),
            _ => 0,
        });

        return new(new(entry.HotspotX, entry.HotspotY), Convert(bitmap));
    }

    private static RawImage Convert(SKBitmap bitmap)
    {
        if (bitmap.ColorType is SKColorType.Rgba8888)
            return new(bitmap.Width, bitmap.Height, bitmap.Bytes);

        using SKBitmap tmpbmp = new(bitmap.Width, bitmap.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using SKCanvas canvas = new(tmpbmp);
        canvas.DrawBitmap(bitmap, 0, 0);
        return new(tmpbmp.Width, tmpbmp.Height, tmpbmp.Bytes);
    }
}