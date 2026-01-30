using System.Diagnostics;

using Shimakaze.UI.Media.Bmp;
using Shimakaze.UI.Media.Internal;

using SkiaSharp;

namespace Shimakaze.UI.Media.Ico;

public static class IcoDecoder
{
    private static readonly byte[] PngMagic = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];

    public static IEnumerable<SKBitmap> Decode(Stream stream)
        => DecodeCore(stream).Select(static i => i.Bitmap);

    internal static IEnumerable<(SKBitmap Bitmap, IconDirEntry Entry, BitmapInfoHeader? BitmapInfo, bool IsPng)> DecodeCore(Stream stream)
    {
        var basePosition = stream.Position;

        stream.Read(out IconDir dir);
        if (dir is not { Reserved: 0, Count: not 0 })
            throw new NotSupportedException();

        IconDirEntry[] entries = new IconDirEntry[dir.Count];
        stream.Read(entries);

        for (int i = 0; i < entries.Length; i++)
        {
            ref var entry = ref entries[i];
            stream.Position = basePosition + entry.ImageOffset;
            byte[] data = new byte[entry.BytesInRes];
            stream.ReadExactly(data);

            if (data.AsSpan(0, 8).SequenceEqual(PngMagic))
            {
                var bmp = SKBitmap.Decode(data);
                Debug.Assert(bmp is not null);
                yield return (bmp, entry, null, true);
            }
            else
            {
                var span = data.AsSpan();
                var header = BmpDecoder.DecodeHeader(span);
                span = span[40..];
                var pixels = BmpDecoder.DecodeData(span, header, out var size, header.BitCount != 32);
                var height = header.Height;
                if (header.BitCount != 32)
                {
                    height /= 2;
                    span = span[size..];
                    BmpDecoder.PatchMask(span, header, pixels, true);
                }

                SKBitmap bmp = new(header.Width, height, SKColorType.Rgba8888, SKAlphaType.Premul)
                {
                    Pixels = pixels
                };
                yield return (bmp, entry, header, false);
            }
        }
    }
}