namespace Shimakaze.UI.Media.Riff;

public sealed record class RiffChunkData(IRiffChunk Chunk, ReadOnlyMemory<byte> Data)
{
    public uint Id => Chunk.Id;
}