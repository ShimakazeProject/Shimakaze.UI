using System.Runtime.InteropServices;

namespace Shimakaze.UI.Media.Bmp;


[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 40)]
public readonly record struct BitmapInfoHeader(
    uint Size,
    int Width,
    int Height,
    ushort Planes,
    ushort BitCount,
    uint Compression,
    uint SizeImage,
    int XPelsPerMeter,
    int YPelsPerMeter,
    uint ClrUsed,
    uint ClrImportant);