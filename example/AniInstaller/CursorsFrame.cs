using SkiaSharp;

sealed record class CursorsFrame
{
    public required SKBitmap Arrow { get; init; }
    public required SKBitmap Help { get; init; }
    public required SKBitmap AppStarting { get; init; }
    public required SKBitmap Wait { get; init; }
    public required SKBitmap Crosshair { get; init; }
    public required SKBitmap IBeam { get; init; }
    public required SKBitmap NWPen { get; init; }
    public required SKBitmap No { get; init; }
    public required SKBitmap SizeNS { get; init; }
    public required SKBitmap SizeWE { get; init; }
    public required SKBitmap SizeNWSE { get; init; }
    public required SKBitmap SizeNESW { get; init; }
    public required SKBitmap SizeAll { get; init; }
    public required SKBitmap UpArrow { get; init; }
    public required SKBitmap Hand { get; init; }
}
