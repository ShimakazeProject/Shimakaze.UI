namespace Shimakaze.UI.Fonts;

public sealed record class FilePathFont(
    string FilePath,
    int Index = 0,
    float Size = 12,
    float ScaleX = 1,
    float SkewX = 0,
    int Weight = FontWeights.Normal,
    int Width = FontWidths.Normal,
    FontStyleSlant Slant = FontStyleSlant.Upright)
    : Font(Size, ScaleX, SkewX, Weight, Width, Slant);