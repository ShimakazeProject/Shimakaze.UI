using System.Drawing;

using Shimakaze.UI.Core;
using Shimakaze.UI.Fonts;
using Shimakaze.UI.Input;
using Shimakaze.UI.Input.EventArgs;
using Shimakaze.UI.Rendering;

using Silk.NET.Input;

sealed class MainWindow(IRendererProvider rendererProvider) : Window
{
    private const float Degrees = -60;
    private const double Tick = 1 / 60d;
    private readonly LinkedList<Cursors> _cursors = [];
    private readonly Font _fontHeader = new("Segoe UI", 12);
    private readonly Font _fontLeftSide = new("Microsoft YaHei UI", 16);
    private readonly Font _fontRightSide = new("Segoe UI", 16);

    private double _total = 0;

    private double _offsetY = 0;
    private const int StartX = 120;
    private const int StartY = 64;
    private int _cursorWidth = 32;
    private int _cursorHeight = 32;
    private float _descentLeft;
    private float _descentRight;

    protected override async void OnInitialize()
    {
        base.OnInitialize();
        _descentLeft = FontManager.GetFont(_fontLeftSide).Metrics.Descent;
        _descentRight = FontManager.GetFont(_fontRightSide).Metrics.Descent;

        INativeWindow native = this;
        var size = native.Native.Size;
        size.X = 720;
        native.Native.Size = size;

        var height = native.Native.Size.Y - StartY;
        var count = height / _cursorWidth + 1;

        this.Input.MouseScroll += Scroll;
        this.Input.MouseClick += Click;

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

    private void Click(InputManager sender, MouseClickEventArgs eventArgs)
    {
        if (eventArgs.Button is not MouseButton.Left)
            return;

        int x = StartX + _cursorWidth * 15;
        if (eventArgs.Position.X < x)
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
        using var renderer = rendererProvider.GetRenderer(this);

        renderer.Clear(Color.Black);

        _total += time;

        int frame = (int)(_total / Tick);

        float y = StartY;

        y += (int)(_offsetY * _cursorHeight);

        PrintHeader(renderer);
        var node = _cursors.First;
        while (node is not null)
        {
            var cursor = node.Value;
            node = node.Next;

            var frames = cursor.GetFrame(frame);
            var draw = y >= StartY;
            if (y > native.Native.Size.Y)
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
                    StartX + _cursorWidth * 15,
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
        rotatedRenderer.DrawText("Arrow", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Help", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("AppStarting", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Wait", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Crosshair", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("IBeam", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("NWPen", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("No", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("SizeNS", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("SizeWE", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("SizeNWSE", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("SizeNESW", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("SizeAll", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("UpArrow", x, StartY, TextAlign.Left, _fontHeader);
        x += _cursorWidth;
        rotatedRenderer.DrawText("Hand", x, StartY, TextAlign.Left, _fontHeader);
    }
}