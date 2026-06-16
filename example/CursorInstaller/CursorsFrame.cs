using Shimakaze.Foundation.Rendering;

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
    public SKBitmap? Person { get; init; }
    public SKBitmap? Pin { get; init; }

    public void Draw(Renderer renderer, float x, float y, in int cursorWidth)
    {
        if (Arrow is not null)
            renderer.DrawBitmap(Arrow, x, y);
        x += cursorWidth;

        if (Help is not null)
            renderer.DrawBitmap(Help, x, y);
        x += cursorWidth;

        if (AppStarting is not null)
            renderer.DrawBitmap(AppStarting, x, y);
        x += cursorWidth;

        if (Wait is not null)
            renderer.DrawBitmap(Wait, x, y);
        x += cursorWidth;

        if (Crosshair is not null)
            renderer.DrawBitmap(Crosshair, x, y);
        x += cursorWidth;

        if (IBeam is not null)
            renderer.DrawBitmap(IBeam, x, y);
        x += cursorWidth;

        if (NWPen is not null)
            renderer.DrawBitmap(NWPen, x, y);
        x += cursorWidth;

        if (No is not null)
            renderer.DrawBitmap(No, x, y);
        x += cursorWidth;

        if (SizeNS is not null)
            renderer.DrawBitmap(SizeNS, x, y);
        x += cursorWidth;

        if (SizeWE is not null)
            renderer.DrawBitmap(SizeWE, x, y);
        x += cursorWidth;

        if (SizeNWSE is not null)
            renderer.DrawBitmap(SizeNWSE, x, y);
        x += cursorWidth;

        if (SizeNESW is not null)
            renderer.DrawBitmap(SizeNESW, x, y);
        x += cursorWidth;

        if (SizeAll is not null)
            renderer.DrawBitmap(SizeAll, x, y);
        x += cursorWidth;

        if (UpArrow is not null)
            renderer.DrawBitmap(UpArrow, x, y);
        x += cursorWidth;

        if (Hand is not null)
            renderer.DrawBitmap(Hand, x, y);
        x += cursorWidth;

        if (Person is not null)
            renderer.DrawBitmap(Person, x, y);
        x += cursorWidth;

        if (Pin is not null)
            renderer.DrawBitmap(Pin, x, y);
        x += cursorWidth;
    }
}