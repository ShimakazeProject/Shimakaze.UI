using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Shimakaze.UI.Input.Cursors;
using Shimakaze.UI.Media.Internal;
using Shimakaze.UI.Media.Riff;

namespace Shimakaze.UI.Media.Cursor;

public static class AniDecoder
{
    public static Input.Cursors.Cursor Decode(Stream stream)
        => Input.Cursors.Cursor.Custom(DecodeCore(stream));

    private static IEnumerable<JiffiesCursorFrame> DecodeCore(Stream stream)
    {
        var basePosition = stream.Position;

        var header = RiffDecoder.ReadChunk(stream);
        if (header is not RiffFourChunk { FourCC: RiffConstants.ACON })
            throw new FormatException("Bad .ani file");

        var chunks = RiffDecoder
            .ReadChunkDataCollection(stream)
            .ToImmutableArray();

        var chunk = chunks.FirstOrDefault(i => i.Id is RiffConstants.anih)
            ?? throw new FormatException("Cannot found chunk anih");
        AniHeader anih = MemoryMarshal.Read<AniHeader>(chunk.Data.Span);

        chunk = chunks.FirstOrDefault(i => i.Chunk is RiffFourChunk { FourCC: RiffConstants.fram })
            ?? throw new FormatException("Cannot found chunk LIST fram");

        var icons = ParseList(chunk.Data);
        Debug.Assert(icons.Length == anih.Frames);

        ReadOnlySpan<int> seq = Enumerable.Range(0, (int)anih.Steps).ToArray();
        chunk = chunks.FirstOrDefault(i => i.Id is RiffConstants.seq_);
        if (chunk is not null)
            seq = MemoryMarshal.Cast<byte, int>(chunk.Data.Span);
        Debug.Assert(seq.Length == anih.Steps);

        ReadOnlySpan<uint> rate = ParseRate(anih);
        chunk = chunks.FirstOrDefault(i => i.Id is RiffConstants.rate);
        if (chunk is not null)
            rate = MemoryMarshal.Cast<byte, uint>(chunk.Data.Span);
        Debug.Assert(rate.Length == anih.Steps);

        List<JiffiesCursorFrame> frames = new((int)anih.Frames);
        for (int i = 0; i < anih.Frames; i++)
        {
            using ReadOnlyMemoryStream ms = new(icons[i].Data);
            var (hotspot, bitmap) = CurDecoder.DecodeCore(ms);
            var jiffies = rate[i];

            JiffiesCursorFrame frame = new(hotspot, bitmap, jiffies);
            frames.Add(frame);
        }

        var sequence = seq.ToArray();
        for (int i = 0; i < anih.Steps; i++)
            yield return frames[sequence[i]];
    }

    private static ImmutableArray<RiffChunkData> ParseList(ReadOnlyMemory<byte> data)
    {
        using ReadOnlyMemoryStream ms = new(data);
        return [.. RiffDecoder.ReadChunkDataCollection(ms, ms.Length)];
    }

    private static ReadOnlySpan<uint> ParseRate(in AniHeader anih)
    {
        uint[] rate = GC.AllocateUninitializedArray<uint>((int)(anih.Steps));
        Array.Fill(rate, anih.JifRate);
        return rate;
    }
}