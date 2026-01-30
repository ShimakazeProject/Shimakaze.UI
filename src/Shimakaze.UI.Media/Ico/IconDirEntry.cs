using System.Runtime.InteropServices;

namespace Shimakaze.UI.Media.Ico;

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]
public readonly record struct IconDirEntry(
    [field: FieldOffset(0)] byte Width,
    [field: FieldOffset(1)] byte Height,
    [field: FieldOffset(2)] byte ColorCount,
    [field: FieldOffset(3)] byte Reserved,
    [field: FieldOffset(4)] ushort Planes,
    [field: FieldOffset(6)] ushort BitCount,
    [field: FieldOffset(8)] uint BytesInRes,
    [field: FieldOffset(12)] uint ImageOffset)
{
    [field: FieldOffset(4)]
    public ushort HotspotX { get; }
    [field: FieldOffset(6)]
    public ushort HotspotY { get; }
}