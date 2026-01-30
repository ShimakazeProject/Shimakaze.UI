using System.Runtime.InteropServices;

namespace Shimakaze.UI.Media.Riff;

public static class RiffDecoder
{
    public static IRiffChunk ReadChunk(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[12];
        stream.ReadExactly(buffer[0..8]);
        var id = buffer[0..4];
        if (!id.SequenceEqual("RIFF"u8) && !id.SequenceEqual("LIST"u8))
            return MemoryMarshal.Read<RiffChunk>(buffer);

        stream.ReadExactly(buffer[8..]);
        return MemoryMarshal.Read<RiffFourChunk>(buffer);
    }

    public static RiffChunkData ReadChunkData(Stream stream)
    {
        var chunk = ReadChunk(stream);
        var size = chunk.Size;
        if (chunk is RiffFourChunk)
            size -= 4;

        byte[] data = GC.AllocateUninitializedArray<byte>(unchecked((int)size));
        stream.ReadExactly(data);

        return new(chunk, data);
    }

    public static IEnumerable<RiffChunkData> ReadChunkDataCollection(Stream stream, long? length = null)
    {
        var end = length.HasValue
            ? stream.Position + length
            : stream.Length;

        while (stream.Position < end)
        {
            var chunk = ReadChunkData(stream);
            yield return chunk;

            if ((chunk.Data.Length & 1) is not 0)
            {
                // Size 不能被 2 整除，将跳过一个字节
                stream.ReadByte();
            }
        }
    }
}