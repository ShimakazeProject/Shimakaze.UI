using SkiaSharp;

sealed record class CursorsFrame
{
    public SKBitmap? Arrow { get; init; }
    public SKBitmap? Help { get; init; }
    public SKBitmap? AppStarting { get; init; }
    public SKBitmap? Wait { get; init; }
    public SKBitmap? Crosshair { get; init; }
    public SKBitmap? IBeam { get; init; }
    public SKBitmap? NWPen { get; init; }
    public SKBitmap? No { get; init; }
    public SKBitmap? SizeNS { get; init; }
    public SKBitmap? SizeWE { get; init; }
    public SKBitmap? SizeNWSE { get; init; }
    public SKBitmap? SizeNESW { get; init; }
    public SKBitmap? SizeAll { get; init; }
    public SKBitmap? UpArrow { get; init; }
    public SKBitmap? Hand { get; init; }

    public void Draw(SKCanvas canvas, ref int x, int y, in int cursorWidth)
    {
        canvas.DrawBitmap(Arrow, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(Help, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(AppStarting, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(Wait, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(Crosshair, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(IBeam, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(NWPen, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(No, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(SizeNS, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(SizeWE, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(SizeNWSE, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(SizeNESW, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(SizeAll, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(UpArrow, x, y);
        x += cursorWidth;

        canvas.DrawBitmap(Hand, x, y);
        x += cursorWidth;
    }
}