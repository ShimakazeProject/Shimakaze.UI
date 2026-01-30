using Silk.NET.Input;

namespace Shimakaze.UI.Input.Cursors;

internal sealed class CustomCursor : Cursor
{
    private readonly IReadOnlyList<CursorFrame> _frames;
    private int _counter;

    public CustomCursor(IEnumerable<CursorFrame> frames) : base()
    {
        List<CursorFrame> frameList = [];
        foreach (var frame in frames)
        {
            if (frame is JiffiesCursorFrame jiffies)
            {
                for (int i = 0; i < jiffies.Jiffies; i++)
                    frameList.Add(frame);
            }
            else
            {
                frameList.Add(frame);
            }
        }

        _frames = [.. frameList];
    }

    internal override async Task Apply(ICursor cursor, CancellationToken cancellationToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1 / 60d));
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            _counter = (_counter + 1) % _frames.Count;

            var frame = _frames[_counter];

            cursor.CursorMode = CursorMode.Normal;
            cursor.Type = CursorType.Custom;
            cursor.HotspotX = frame.Hotspot.X;
            cursor.HotspotY = frame.Hotspot.Y;
            cursor.Image = frame.Image;
        }
    }
}