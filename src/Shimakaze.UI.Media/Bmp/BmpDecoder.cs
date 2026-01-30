using System.Runtime.InteropServices;

using SkiaSharp;

namespace Shimakaze.UI.Media.Bmp;

public sealed class BmpDecoder
{
    public static BitmapInfoHeader DecodeHeader(ReadOnlySpan<byte> data)
    {
        return MemoryMarshal.Read<BitmapInfoHeader>(data);
    }

    public static SKColor[] DecodeData(ReadOnlySpan<byte> data, in BitmapInfoHeader header, out int size, bool isDoubleHeight = false)
    {
        int width = header.Width;
        int height = header.Height;
        if (isDoubleHeight)
            height /= 2;  // ICO/CUR 特有
        int bpp = header.BitCount;

        int stride = ((width * bpp + 31) / 32) * 4;

        size = stride * height;
        SKColor[] bmp = new SKColor[width * height];
        switch (bpp)
        {
            case 32:
                DecodeBgra32(data, bmp, height, width, stride);
                break;
            case 24:
                size = stride * height;
                DecodeBgr24(data, bmp, height, width, stride);
                break;
            case 8:
            case 4:
            case 1:
                size += (1 << bpp) * 4;
                DecodePaletteData(data, bmp, height, width, stride, bpp);
                break;
            default:
                throw new NotSupportedException();
        }

        return bmp;
    }

    public static void PatchMask(ReadOnlySpan<byte> data, in BitmapInfoHeader header, SKColor[] pixels, bool isDoubleHeight = false)
    {
        int width = header.Width;
        int height = header.Height;
        if (isDoubleHeight)
            height /= 2;  // ICO/CUR 特有

        int stride = ((width + 31) / 32) * 4;

        for (int invertedY = 0; invertedY < height; invertedY++)
        {
            int y = height - 1 - invertedY;
            var row = data.Slice(invertedY * stride, stride);
            for (int x = 0; x < width; x++)
            {
                var i = GetIndex(row, x, 1);
                if (i is 1) // 透明
                {
                    ref var pixel = ref pixels[y * width + x];
                    var span = MemoryMarshal.CreateSpan(ref pixel, 1);
                    var tmp = MemoryMarshal.Cast<SKColor, byte>(span);
                    tmp[3] = 0;
                }
            }
        }
    }

    private static void DecodePaletteData(ReadOnlySpan<byte> data, SKColor[] pixels, int height, int width, int stride, int bpp)
    {
        var palette = new SKColor[1 << bpp];
        for (int i = 0; i < palette.Length; i++)
        {
            palette[i] = new(data[2], data[1], data[0]);
            data = data[4..];
        }

        for (int invertedY = 0; invertedY < height; invertedY++)
        {
            int y = height - 1 - invertedY;
            var row = data[(invertedY * stride)..];
            for (int x = 0; x < width; x++)
            {
                pixels[y * width + x] = palette[GetIndex(row, x, bpp)];
            }
        }
    }

    private static byte GetIndex(ReadOnlySpan<byte> row, int index, int bpp)
    {
        if (bpp is 8)
            return row[index];

        var bitIndex = index * bpp;
        var byteIndex = bitIndex / 8;
        var bitOffset = bitIndex % 8;
        int shift = 8 - bpp - bitOffset;

        byte mask = 0;
        for (int i = 0; i < bpp; i++)
        {
            mask <<= 1;
            mask |= 1;
        }

        var value = row[byteIndex];
        value >>= shift;

        value &= mask;
        return value;
    }

    private static void DecodeBgr24(ReadOnlySpan<byte> data, SKColor[] pixels, int height, int width, int stride)
    {
        for (int invertedY = 0; invertedY < height; invertedY++)
        {
            int y = height - 1 - invertedY;
            for (int x = 0; x < width; x++)
            {
                var i = (invertedY * stride) + (x * 3);
                pixels[y * width + x] = new(data[i + 2], data[i + 1], data[i]);
            }
        }
    }

    private static void DecodeBgra32(ReadOnlySpan<byte> data, SKColor[] pixels, int height, int width, int stride)
    {
        for (int invertedY = 0; invertedY < height; invertedY++)
        {
            int y = height - 1 - invertedY;
            for (int x = 0; x < width; x++)
            {
                var i = (invertedY * stride) + (x * 4);
                pixels[y * width + x] = new(data[i + 2], data[i + 1], data[i], data[i + 3]);
            }
        }
    }
}