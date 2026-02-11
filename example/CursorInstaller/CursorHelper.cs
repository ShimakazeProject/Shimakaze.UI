using Shimakaze.UI.Media.Ani;
using Shimakaze.UI.Media.Cur;

using SkiaSharp;

static class CursorHelper
{
    public static async IAsyncEnumerable<Cursors> LoadCursor(string folderPath)
    {
        foreach (var folder in Directory.EnumerateDirectories(folderPath))
        {
            var files = Directory
                .EnumerateFiles(folder)
                .Where(static i =>
                {
                    var ext = Path.GetExtension(i);
                    return string.Equals(ext, ".ani", StringComparison.OrdinalIgnoreCase) || string.Equals(ext, ".cur", StringComparison.OrdinalIgnoreCase);
                });

            if (!files.Any())
            {
                await foreach (var item in LoadCursor(folder))
                    yield return item;

                continue;
            }

            ReadOnlySpan<char> name = Path.GetFileName(folder);
            if (name.EndsWith("マウスカーソル"))
                name = name[..^7];
            if (name.IndexOf('_') is int i and not -1)
                name = name[(i + 1)..];

            Cursors cursors = new(name.ToString());
            foreach (var file in files)
            {
                var cursor = LoadFrameAsync(file);
                ApplyToCollection(file, await cursor.ToListAsync(), cursors);
            }

            yield return cursors;
        }
    }

    private static async IAsyncEnumerable<SKBitmap> LoadFrameAsync(string path)
    {
        await using var fs = File.OpenRead(path);
        if (string.Equals(Path.GetExtension(path), ".ani", StringComparison.OrdinalIgnoreCase))
        {
            foreach (var (bitmap, jiffies) in AniDecoder.DecodeFrames(fs))
            {
                for (uint i = 0; i < jiffies; i++)
                    yield return bitmap;
            }
        }
        else
        {
            CurDecoder.Decode(fs, out var bitmap, out _);
            yield return bitmap;
        }
    }

    private static void ApplyToCollection(string filePath, IReadOnlyList<SKBitmap> cursor, Cursors cursors)
    {
        switch (Path.GetFileNameWithoutExtension(filePath))
        {
            case "通常":
            case "通常の選択":
            case "Arrow":
            case "Normal":
                cursors.Arrow = cursor;
                cursors.ArrowPath = filePath;
                break;
            case "ヘルプの選択":
            case "Help":
                cursors.Help = cursor;
                cursors.HelpPath = filePath;
                break;
            case "バックグラウンドで作業中":
            case "AppStarting":
            case "Working":
                cursors.AppStarting = cursor;
                cursors.AppStartingPath = filePath;
                break;
            case "待ち状態":
            case "Wait":
            case "Busy":
                cursors.Wait = cursor;
                cursors.WaitPath = filePath;
                break;
            case "領域選択":
            case "Crosshair":
            case "Precision":
                cursors.Crosshair = cursor;
                cursors.CrosshairPath = filePath;
                break;
            case "テキスト選択":
            case "IBeam":
            case "Text":
                cursors.IBeam = cursor;
                cursors.IBeamPath = filePath;
                break;
            case "手書き":
            case "NWPen":
            case "Handwriting":
                cursors.NWPen = cursor;
                cursors.NWPenPath = filePath;
                break;
            case "利用不可":
            case "No":
            case "Unavailable":
                cursors.No = cursor;
                cursors.NoPath = filePath;
                break;
            case "上下に拡大縮小":
            case "SizeNS":
            case "Vertical":
                cursors.SizeNS = cursor;
                cursors.SizeNSPath = filePath;
                break;
            case "左右に拡大縮小":
            case "SizeWE":
            case "Horizontal":
                cursors.SizeWE = cursor;
                cursors.SizeWEPath = filePath;
                break;
            case "斜めに拡大縮小1":
            case "SizeNWSE":
            case "Diagonal1":
                cursors.SizeNWSE = cursor;
                cursors.SizeNWSEPath = filePath;
                break;
            case "斜めに拡大縮小2":
            case "SizeNESW":
            case "Diagonal2":
                cursors.SizeNESW = cursor;
                cursors.SizeNESWPath = filePath;
                break;
            case "移動":
            case "SizeAll":
            case "Move":
                cursors.SizeAll = cursor;
                cursors.SizeAllPath = filePath;
                break;
            case "代替選択":
            case "UpArrow":
            case "Alternate":
                cursors.UpArrow = cursor;
                cursors.UpArrowPath = filePath;
                break;
            case "リンクの選択":
            case "Hand":
            case "Link":
                cursors.Hand = cursor;
                cursors.HandPath = filePath;
                break;
            case "Person":
                cursors.Person = cursor;
                cursors.PersonPath = filePath;
                break;
            case "Pin":
                cursors.Pin = cursor;
                cursors.PinPath = filePath;
                break;
            default:
                break;
        }
    }

}