using System.Drawing;

using Shimakaze.UI.Core;
using Shimakaze.UI.Fonts;
using Shimakaze.UI.Input;
using Shimakaze.UI.Input.EventArgs;
using Shimakaze.UI.Rendering;
using Shimakaze.UI.Rendering.Extensions;

using Silk.NET.Input;

using SkiaSharp;

sealed class MainWindow : PlatformWindow
{
    private const float DAMPING = 8.0f;  // 阻尼系数（单位：1/秒），典型值 6~12

    private const float Degrees = -60;
    private const double Tick = 1 / 60d;
    private readonly LinkedList<Cursors> _cursors = [];
    private readonly Font _fontHeader = Font.FromFamilyName("Segoe UI", 12);
    private readonly Font _fontLeftSide = Font.FromFamilyName("Microsoft YaHei UI", 16);
    private readonly Font _fontRightSide = Font.FromFamilyName("Segoe UI", 16);

    private double _total = 0;

    private const int StartX = 120;
    private const int StartY = 64;
    private int _cursorWidth = 32;
    private int _cursorHeight = 32;
    private float _descentLeft;
    private float _descentRight;

    private float _currentY = 0;
    private float _targetY = 0;

    private int _focus = 0;
    private float _currentFocusY = 0;
    private float _targetFocusY = 0;
    private readonly SKPaint _focusPaint = new()
    {
        Color = Color.FromArgb(0x7F808080).ToSkia(),
    };


    protected override async void OnInitialize()
    {
        base.OnInitialize();
        _descentLeft = FontManager.GetFont(_fontLeftSide).Metrics.Descent;
        _descentRight = FontManager.GetFont(_fontRightSide).Metrics.Descent;

        var size = Native.Size;
        size.X = 720;
        Native.Size = size;

        var height = Size.Height - StartY;
        var count = height / _cursorWidth + 1;

        this.Input.MouseScroll += Scroll;
        this.Input.MouseClick += Click;
        this.Keyboard.KeyPressed += KeyPressed;

        await Task.Run(async () =>
        {
            await foreach (var cursor in CursorHelper.LoadCursor(Path.Combine(AppContext.BaseDirectory, "cursors")))
            {
                _cursors.AddLast(cursor);

                foreach (var bitmap in cursor.GetAllBitmaps())
                {
                    _cursorWidth = Math.Max(_cursorWidth, bitmap.Width);
                    _cursorHeight = Math.Max(_cursorHeight, bitmap.Height);
                }
            }
        }).ConfigureAwait(false);
    }

    private void KeyPressed(KeyboardManager sender, KeyboardKeyEventArgs eventArgs)
    {
        switch (eventArgs.Key)
        {
            case Key.Home:
                _focus = 0;
                break;
            case Key.End:
                _focus = int.MaxValue;
                break;
            case Key.Up:
                _focus--;
                break;
            case Key.Down:
                _focus++;
                break;
        }

        _focus = int.Clamp(_focus, 0, _cursors.Count - 1);
        Focus();
    }

    private void Click(InputManager sender, MouseClickEventArgs eventArgs)
    {
        if (eventArgs.Button is not MouseButton.Left)
            return;

        int x = StartX + _cursorWidth * 15;
        if (eventArgs.Position.X < x)
            return;

        // var y = eventArgs.Position.Y;
        float y = StartY + _currentY;

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
        _targetY += eventArgs.Wheel.Y * _cursorHeight;
        _targetY = float.Clamp(_targetY, -(_cursors.Count - 1) * _cursorHeight, 0);
    }

    private void Focus()
    {
        var height = Size.Height;
        height -= StartY;

        _targetFocusY = _focus * _cursorHeight;

        _targetY = _targetFocusY - (height - _cursorHeight) / 2;
        _targetY = -_targetY;
        _targetY = float.Clamp(_targetY, -(_cursors.Count - 1) * _cursorHeight, 0);
    }

    protected override void OnUpdate(double deltaTime)
    {
        base.OnUpdate(deltaTime);

        float dt = (float)deltaTime;

        CalcTargetY(dt, ref _currentY, _targetY);
        CalcTargetY(dt, ref _currentFocusY, _targetFocusY);
    }

    private void CalcTargetY(float deltaTime, ref float current, float target)
    {
        if (float.Abs(target - current) <= 0.1f)
        {
            current = target; // 对齐，避免抖动
            return;
        }

        // 指数趋近公式（一阶低通滤波器 / 阻尼运动）
        float t = 1.0f - float.Exp(-DAMPING * deltaTime);
        current += (target - current) * t;

    }

    protected override void OnRender(double time)
    {
        base.OnRender(time);

        using var renderer = Application.GetRenderer(this);

        renderer.Clear(Color.Black);

        _total += time;

        int frame = (int)(_total / Tick);

        float y = StartY + _currentY;

        PrintHeader(renderer);

        using var clip = renderer.ClipRect(RectangleF.FromLTRB(0, StartY, Size.Width, Size.Height));


        renderer.Canvas.DrawRect(
            new RectangleF(0, StartY + _currentFocusY + _currentY, Size.Width, _cursorHeight).ToSkia(),
            _focusPaint);

        var node = _cursors.First;
        while (node is not null)
        {
            var cursor = node.Value;
            node = node.Next;

            var frames = cursor.GetFrame(frame);
            var draw = y > StartY - _cursorHeight;
            if (y > Size.Height)
                break;

            if (draw)
            {
                renderer.DrawText(
                    cursor.Name,
                    StartX,
                    y + (_cursorHeight / 2) + _descentLeft,
                    TextAlign.Right,
                    _fontLeftSide);

                frames.Draw(renderer, StartX, y, _cursorWidth);

                renderer.DrawText(
                    "Apply",
                    StartX + _cursorWidth * 17,
                    y + (_cursorHeight / 2) + _descentRight,
                    TextAlign.Left,
                    _fontRightSide);
            }

            y += _cursorHeight;
        }
    }

    private void PrintHeader(BaseRenderer renderer)
    {
        using var rotatedRenderer = renderer.RotateDegrees(Degrees);

        float x = StartX + _cursorWidth / 2;
        rotatedRenderer.DrawText("Normal", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Help", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Working", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Busy", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Precision", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Text", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Handwriting", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Unavailable", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Vertical", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Horizontal", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Diagonal1", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Diagonal2", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Move", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Alternate", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Link", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Person", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Pin", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
    }
}