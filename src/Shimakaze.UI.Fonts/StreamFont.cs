namespace Shimakaze.UI.Fonts;

public sealed record class StreamFont(
    Stream Stream,
    int Index = 0,
    bool LeaveOpen = false,
    float Size = 12,
    float ScaleX = 1,
    float SkewX = 0,
    int Weight = FontWeights.Normal,
    int Width = FontWidths.Normal,
    FontStyleSlant Slant = FontStyleSlant.Upright)
    : Font(Size, ScaleX, SkewX, Weight, Width, Slant)
{
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            if (!LeaveOpen)
                Stream?.Dispose();
        }
    }
}