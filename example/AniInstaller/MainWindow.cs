using Shimakaze.UI.Core;
using Shimakaze.UI.Input;
using Shimakaze.UI.Input.EventArgs;
using Shimakaze.UI.Media.Ani;
using Shimakaze.UI.Rendering;

using SkiaSharp;
using SkiaSharp.HarfBuzz;

sealed class MainWindow(IRenderer renderer) : Window
{
    private readonly List<Cursors> _cursors = [];

    private readonly double _tick = 1 / 60d;
    private double _total = 0;

    private double _offsetY = 0;
    private const int StartX = 120;
    private const int StartY = 64;
    private const int Block = 32;
    private SKShaper? _shaper;
    private SKFont? _fontHeader;
    private SKFont? _font16;
    private SKPaint? _paint;
    private float _descent;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        using var typeface = SKTypeface.FromFamilyName("Microsoft YaHei UI");
        _fontHeader = new(typeface, 12)
        {
            SkewX = -0.1f
        };
        _font16 = new(typeface, 16);
        _descent = _font16.Metrics.Descent;
        _shaper = new(typeface);
        _paint = new()
        {
            Color = SKColors.White,
            IsAntialias = true,
        };

        INativeWindow native = this;
        var size = native.Native.Size;
        size.X = 600;
        native.Native.Size = size;

        var height = native.Native.Size.Y - StartY;
        var count = height / Block + 1;

        this.Input.MouseScroll += Scroll;
        _cursors.AddRange(LoadCursor(@""));
    }

    private void Scroll(InputManager sender, MouseScrollEventArgs eventArgs)
    {
        _offsetY += eventArgs.Wheel.Y;
        _offsetY = Math.Min(_offsetY, 0);
        _offsetY = Math.Max(_offsetY, -_cursors.Count + 1);
    }

    protected override void OnRender(double time)
    {
        INativeWindow native = this;
        var surface = renderer.GetSurface(this);
        surface.Canvas.Clear(SKColors.Black);

        _total += time;

        int frame = (int)(_total / _tick);

        int y = StartY, x;

        y += (int)(_offsetY * Block);


        PrintHeader(surface.Canvas);
        foreach (var cursor in _cursors)
        {
            var frames = cursor.GetFrame(frame);
            var draw = y >= 64;
            if (y > native.Native.Size.Y)
                break;

            if (draw)
            {
                x = StartX;
                surface.Canvas.DrawBitmap(frames.Arrow, x, y);
                x += frames.Arrow.Width;
                surface.Canvas.DrawBitmap(frames.Help, x, y);
                x += frames.Help.Width;
                surface.Canvas.DrawBitmap(frames.AppStarting, x, y);
                x += frames.AppStarting.Width;
                surface.Canvas.DrawBitmap(frames.Wait, x, y);
                x += frames.Wait.Width;
                surface.Canvas.DrawBitmap(frames.Crosshair, x, y);
                x += frames.Crosshair.Width;
                surface.Canvas.DrawBitmap(frames.IBeam, x, y);
                x += frames.IBeam.Width;
                surface.Canvas.DrawBitmap(frames.NWPen, x, y);
                x += frames.NWPen.Width;
                surface.Canvas.DrawBitmap(frames.No, x, y);
                x += frames.No.Width;
                surface.Canvas.DrawBitmap(frames.SizeNS, x, y);
                x += frames.SizeNS.Width;
                surface.Canvas.DrawBitmap(frames.SizeWE, x, y);
                x += frames.SizeWE.Width;
                surface.Canvas.DrawBitmap(frames.SizeNWSE, x, y);
                x += frames.SizeNWSE.Width;
                surface.Canvas.DrawBitmap(frames.SizeNESW, x, y);
                x += frames.SizeNESW.Width;
                surface.Canvas.DrawBitmap(frames.SizeAll, x, y);
                x += frames.SizeAll.Width;
                surface.Canvas.DrawBitmap(frames.UpArrow, x, y);
                x += frames.UpArrow.Width;
                surface.Canvas.DrawBitmap(frames.Hand, x, y);
                x += frames.Hand.Width;
            }

            var height = new[]
            {
                frames.Arrow.Height,
                frames.Help.Height,
                frames.AppStarting.Height,
                frames.Wait.Height,
                frames.Crosshair.Height,
                frames.IBeam.Height,
                frames.NWPen.Height,
                frames.No.Height,
                frames.SizeNS.Height,
                frames.SizeWE.Height,
                frames.SizeNWSE.Height,
                frames.SizeNESW.Height,
                frames.SizeAll.Height,
                frames.UpArrow.Height,
                frames.Hand.Height,
            }.Max();
            y += height;

            if (draw)
                surface.Canvas.DrawShapedText(_shaper, cursor.Name, StartX, y - (height / 2) + _descent, SKTextAlign.Right, _font16, _paint);
        }

        surface.Flush();
    }

    private void PrintHeader(SKCanvas canvas)
    {
        const float degrees = 60;
        float x, y;
        canvas.Save();
        canvas.Translate(0, 0);

        canvas.RotateDegrees(-degrees);
        (x, y) = RotatePointDegrees(StartX + (Block * (1 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Arrow", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (2 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Help", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (3 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "AppStarting", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (4 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Wait", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (5 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Crosshair", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (6 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "IBeam", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (7 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "NWPen", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (8 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "No", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (9 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeNS", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (10 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeWE", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (11 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeNWSE", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (12 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeNESW", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (13 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeAll", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (14 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "UpArrow", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (Block * (15 - 1)) + (Block / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Hand", x, y, SKTextAlign.Left, _fontHeader, _paint);
        canvas.RotateDegrees(degrees);
    }


    /// <summary>
    /// 绕原点(0,0)旋转坐标点
    /// </summary>
    /// <param name="x">原始X坐标</param>
    /// <param name="y">原始Y坐标</param>
    /// <param name="radians">旋转弧度（正值逆时针，负值顺时针）</param>
    /// <returns>旋转后的坐标(x, y)</returns>
    public static (float x, float y) RotatePoint(float x, float y, float radians)
    {
        // 旋转公式：
        // x' = x * cos(θ) - y * sin(θ)
        // y' = x * sin(θ) + y * cos(θ)
        float cosTheta = (float)Math.Cos(radians);
        float sinTheta = (float)Math.Sin(radians);

        float newX = x * cosTheta - y * sinTheta;
        float newY = x * sinTheta + y * cosTheta;

        return (newX, newY);
    }

    /// <summary>
    /// 绕原点(0,0)旋转坐标点（使用度数的版本）
    /// </summary>
    /// <param name="x">原始X坐标</param>
    /// <param name="y">原始Y坐标</param>
    /// <param name="degrees">旋转角度（正值逆时针，负值顺时针）</param>
    /// <returns>旋转后的坐标(x, y)</returns>
    public static (float x, float y) RotatePointDegrees(float x, float y, float degrees)
    {
        // 将角度转换为弧度：弧度 = 角度 × π / 180
        float radians = degrees * (float)Math.PI / 180f;
        return RotatePoint(x, y, radians);
    }

    private static IEnumerable<Cursors> LoadCursor(string folderPath)
    {
        foreach (var folder in Directory.EnumerateDirectories(folderPath))
        {
            var files = Directory.GetFiles(folder, "*.ani");
            if (files.Length is 0)
            {
                foreach (var item in LoadCursor(folder))
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
                var cursor = LoadFrame(file);
                ApplyToCollection(file, [.. cursor], cursors);
            }

            yield return cursors;
        }
    }

    private static IEnumerable<SKBitmap> LoadFrame(string path)
    {
        using var fs = File.OpenRead(path);
        foreach (var (bitmap, jiffies) in AniDecoder.DecodeFrames(fs))
        {
            for (uint i = 0; i < jiffies; i++)
                yield return bitmap;
        }
    }

    private static void ApplyToCollection(string fileName, IReadOnlyList<SKBitmap> cursor, Cursors cursors)
    {
        switch (Path.GetFileNameWithoutExtension(fileName))
        {
            case "通常":
            case "通常の選択":
                cursors.Arrow = cursor;
                break;
            case "ヘルプの選択":
                cursors.Help = cursor;
                break;
            case "バックグラウンドで作業中":
                cursors.AppStarting = cursor;
                break;
            case "待ち状態":
                cursors.Wait = cursor;
                break;
            case "領域選択":
                cursors.Crosshair = cursor;
                break;
            case "テキスト選択":
                cursors.IBeam = cursor;
                break;
            case "手書き":
                cursors.NWPen = cursor;
                break;
            case "利用不可":
                cursors.No = cursor;
                break;
            case "上下に拡大縮小":
                cursors.SizeNS = cursor;
                break;
            case "左右に拡大縮小":
                cursors.SizeWE = cursor;
                break;
            case "斜めに拡大縮小1":
                cursors.SizeNWSE = cursor;
                break;
            case "斜めに拡大縮小2":
                cursors.SizeNESW = cursor;
                break;
            case "移動":
                cursors.SizeAll = cursor;
                break;
            case "代替選択":
                cursors.UpArrow = cursor;
                break;
            case "リンクの選択":
                cursors.Hand = cursor;
                break;
            default:
                break;
        }
    }


}