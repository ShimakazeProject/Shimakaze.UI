using System.Runtime.InteropServices;
using System.Text;

namespace Shimakaze.UI.Media.Riff;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly record struct RiffChunk(
    [field: FieldOffset(0)] uint Id,
    [field: FieldOffset(4)] uint Size) : IRiffChunk
{
    internal static string GetASCII(in uint id) => Encoding.ASCII.GetString(BitConverter.GetBytes(id));

    public override string ToString() => $"Id: {GetASCII(Id)} Size: {Size}";
}