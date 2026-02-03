using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Shimakaze.UI.Media.Cur;
using Shimakaze.UI.Media.Internal;
using Shimakaze.UI.Media.Riff;

using SkiaSharp;

namespace Shimakaze.UI.Media.Ani;

public static class AniDecoder
{
    public static IEnumerable<(SKBitmap Bitmap, uint Jiffies, SKPointI HotSpot)> DecodeFramesWithMetadata(Stream stream, CursorSelector? selector = null)
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


        List<(SKBitmap Bitmap, uint Jiffies, SKPointI HotSpot)> frames = new((int)anih.Frames);
        for (int i = 0; i < anih.Frames; i++)
        {
            using ReadOnlyMemoryStream ms = new(icons[i].Data);
            CurDecoder.Decode(ms, out var bitmap, out var hotspot, selector);
            var jiffies = rate[i];

            frames.Add(new(bitmap, jiffies, hotspot));
        }

        var sequence = seq.ToArray();
        for (int i = 0; i < anih.Steps; i++)
            yield return frames[sequence[i]];
    }

    public static IEnumerable<(SKBitmap Bitmap, uint Jiffies)> DecodeFrames(Stream stream, CursorSelector? selector = null)
    {
        foreach (var (bitmap, jiffies, _) in DecodeFramesWithMetadata(stream, selector))
            yield return (bitmap, jiffies);
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