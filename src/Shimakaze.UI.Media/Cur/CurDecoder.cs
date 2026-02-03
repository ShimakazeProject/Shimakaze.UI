
using System.Collections;

using Shimakaze.UI.Media.Bmp;
using Shimakaze.UI.Media.Ico;

using SkiaSharp;

namespace Shimakaze.UI.Media.Cur;

public delegate int CursorSelector(SKBitmap bitmap, in IconDirEntry entry, in BitmapInfoHeader? bitmapInfo, bool isPng);

public static class CurDecoder
{
    public static IEnumerable<(SKBitmap Bitmap, IconDirEntry Entry, BitmapInfoHeader? BitmapInfo, bool IsPng)> DecodeWithMetadata(Stream stream)
        => IcoDecoder.DecodeWithMetadata(stream);

    public static IEnumerable<(SKBitmap Bitmap, SKPointI Hotspot)> DecodeWithHotspot(Stream stream)
    {
        foreach (var (bitmap, entry, _, _) in DecodeWithMetadata(stream))
            yield return (bitmap, new(entry.HotspotX, entry.HotspotY));
    }

    public static IEnumerable<SKBitmap> Decode(Stream stream)
    {
        foreach (var (bitmap, _) in DecodeWithHotspot(stream))
            yield return bitmap;
    }

    public static void Decode(Stream stream, out SKBitmap bitmap, out SKPointI hotspot, CursorSelector? selector = null)
    {
        selector ??= DefaultSelector;

        (bitmap, var entry, _, _) = DecodeWithMetadata(stream)
            .MaxBy(i => selector(i.Bitmap, i.Entry, i.BitmapInfo, i.IsPng));

        hotspot = new(entry.HotspotX, entry.HotspotY);
    }

    public static int DefaultSelector(SKBitmap bitmap, in IconDirEntry entry, in BitmapInfoHeader? bitmapInfo, bool isPng)
    {
        if (isPng)
            return int.MaxValue;

        if (bitmapInfo.HasValue)
            return bitmapInfo.Value.Width * bitmapInfo.Value.Height * (bitmapInfo.Value.BitCount / 8);

        return 0;
    }
}