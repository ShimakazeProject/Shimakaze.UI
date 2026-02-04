namespace Shimakaze.UI.Fonts;

public sealed record class Font(
    string FamilyName,
    float Size = 12,
    float ScaleX = 1,
    float SkewX = 0,
    int Weight = FontWeights.Normal,
    int Width = FontWidths.Normal,
    FontStyleSlant Slant = FontStyleSlant.Upright) : IDisposable
{
    internal event Action? Disposed;

    public void Dispose() => Disposed?.Invoke();
}