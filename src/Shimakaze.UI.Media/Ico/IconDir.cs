using System.Runtime.InteropServices;

namespace Shimakaze.UI.Media.Ico;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6)]
public readonly record struct IconDir(ushort Reserved, IconType Type, ushort Count);