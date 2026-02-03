using Microsoft.Extensions.Logging;

using Shimakaze.UI.Core;
using Shimakaze.UI.Input;
using Shimakaze.UI.Input.EventArgs;
using Shimakaze.UI.Rendering;

using Silk.NET.Input;

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
    private int _cursorWidth = 32;
    private int _cursorHeight = 32;
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
        var count = height / _cursorWidth + 1;

        this.Input.MouseScroll += Scroll;
        this.Input.MouseDoubleClick += DoubleClick;
        _cursors.AddRange(CursorHelper.LoadCursor(Path.Combine(AppContext.BaseDirectory, "cursors")));

        var bitmaps = _cursors
            .SelectMany(static i => i.GetAllFrames().SelectMany(static i => i));

        _cursorWidth = Math.Max(32, bitmaps.Max(i => i.Width));
        _cursorHeight = Math.Max(32, bitmaps.Max(i => i.Height));
    }

    private void DoubleClick(InputManager sender, MouseClickEventArgs eventArgs)
    {
        if (eventArgs.Button is not MouseButton.Left)
            return;

        // var y = eventArgs.Position.Y;
        int y = StartY;
        y += (int)(_offsetY * _cursorHeight);

        foreach (var cursor in _cursors)
        {
            try
            {
                if (y < StartY)
                    continue;

                if (eventArgs.Position.Y >= y && eventArgs.Position.Y < y + _cursorHeight)
                {
                    cursor.Install();
                    break;
                }
            }
            finally
            {
                y += _cursorHeight;
            }
        }

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

        y += (int)(_offsetY * _cursorHeight);

        PrintHeader(surface.Canvas);
        foreach (var cursor in _cursors)
        {
            var frames = cursor.GetFrame(frame);
            var draw = y >= StartY;
            if (y > native.Native.Size.Y)
                break;

            if (draw)
            {
                x = StartX;
                surface.Canvas.DrawShapedText(
                    _shaper,
                    cursor.Name,
                    x,
                    y + (_cursorHeight / 2) + _descent,
                    SKTextAlign.Right,
                    _font16,
                    _paint);

                surface.Canvas.DrawBitmap(frames.Arrow, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.Help, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.AppStarting, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.Wait, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.Crosshair, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.IBeam, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.NWPen, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.No, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.SizeNS, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.SizeWE, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.SizeNWSE, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.SizeNESW, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.SizeAll, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.UpArrow, x, y);
                x += _cursorWidth;
                surface.Canvas.DrawBitmap(frames.Hand, x, y);
                x += _cursorWidth;
            }

            y += _cursorHeight;
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
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (1 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Arrow", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (2 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Help", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (3 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "AppStarting", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (4 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Wait", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (5 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "Crosshair", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (6 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "IBeam", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (7 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "NWPen", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (8 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "No", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (9 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeNS", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (10 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeWE", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (11 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeNWSE", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (12 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeNESW", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (13 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "SizeAll", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (14 - 1)) + (_cursorWidth / 2), StartY, degrees);
        canvas.DrawShapedText(_shaper, "UpArrow", x, y, SKTextAlign.Left, _fontHeader, _paint);
        (x, y) = RotatePointDegrees(StartX + (_cursorWidth * (15 - 1)) + (_cursorWidth / 2), StartY, degrees);
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
}