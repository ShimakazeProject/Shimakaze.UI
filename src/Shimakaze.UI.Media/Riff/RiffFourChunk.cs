using System.Runtime.InteropServices;

namespace Shimakaze.UI.Media.Riff;

/// <summary>
/// Riff 文件块
/// </summary>
/// <param name="Id">固定为 'RIFF'</param>
/// <param name="Size">文件总大小 - 8（即从偏移 8 到文件末尾的字节数，小端序）</param>
/// <param name="FourCC">文件类型标识（FourCC）</param>
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 12)]
public readonly record struct RiffFourChunk(
    [field: FieldOffset(0)] uint Id,
    [field: FieldOffset(4)] uint Size,
    [field: FieldOffset(8)] uint FourCC) : IRiffChunk
{
    [field: FieldOffset(0)]
    public RiffChunk Base { get; }

    public static implicit operator RiffChunk(RiffFourChunk chunk) => chunk.Base;

    public override string ToString() => $"Id: {RiffChunk.GetASCII(Id)} Size: {Size} Type: {RiffChunk.GetASCII(FourCC)}";
}