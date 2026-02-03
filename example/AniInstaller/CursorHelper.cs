using Shimakaze.UI.Media.Ani;

using SkiaSharp;

static class CursorHelper
{
    public static async IAsyncEnumerable<Cursors> LoadCursor(string folderPath)
    {
        foreach (var folder in Directory.EnumerateDirectories(folderPath))
        {
            var files = Directory.GetFiles(folder, "*.ani");
            if (files.Length is 0)
            {
                await foreach (var item in LoadCursor(folder))
                    yield return item;

                continue;
            }

            ReadOnlySpan<char> name = Path.GetFileName(folder);
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
        foreach (var (bitmap, jiffies) in AniDecoder.DecodeFrames(fs))
        {
            for (uint i = 0; i < jiffies; i++)
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
                cursors.AppStarting = cursor;
                cursors.AppStartingPath = filePath;
                break;
            case "待ち状態":
            case "Wait":
                cursors.Wait = cursor;
                cursors.WaitPath = filePath;
                break;
            case "領域選択":
            case "Crosshair":
                cursors.Crosshair = cursor;
                cursors.CrosshairPath = filePath;
                break;
            case "テキスト選択":
            case "IBeam":
                cursors.IBeam = cursor;
                cursors.IBeamPath = filePath;
                break;
            case "手書き":
            case "NWPen":
                cursors.NWPen = cursor;
                cursors.NWPenPath = filePath;
                break;
            case "利用不可":
            case "No":
                cursors.No = cursor;
                cursors.NoPath = filePath;
                break;
            case "上下に拡大縮小":
            case "SizeNS":
                cursors.SizeNS = cursor;
                cursors.SizeNSPath = filePath;
                break;
            case "左右に拡大縮小":
            case "SizeWE":
                cursors.SizeWE = cursor;
                cursors.SizeWEPath = filePath;
                break;
            case "斜めに拡大縮小1":
            case "SizeNWSE":
                cursors.SizeNWSE = cursor;
                cursors.SizeNWSEPath = filePath;
                break;
            case "斜めに拡大縮小2":
            case "SizeNESW":
                cursors.SizeNESW = cursor;
                cursors.SizeNESWPath = filePath;
                break;
            case "移動":
            case "SizeAll":
                cursors.SizeAll = cursor;
                cursors.SizeAllPath = filePath;
                break;
            case "代替選択":
            case "UpArrow":
                cursors.UpArrow = cursor;
                cursors.UpArrowPath = filePath;
                break;
            case "リンクの選択":
            case "Hand":
                cursors.Hand = cursor;
                cursors.HandPath = filePath;
                break;
            default:
                break;
        }
    }

}