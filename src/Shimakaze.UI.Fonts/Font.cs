namespace Shimakaze.UI.Fonts;

public abstract record class Font(
    float Size = 12,
    float ScaleX = 1,
    float SkewX = 0,
    int Weight = FontWeights.Normal,
    int Width = FontWidths.Normal,
    FontStyleSlant Slant = FontStyleSlant.Upright) : IDisposable
{
    private bool _disposedValue;

    internal event Action? Disposed;

    public static Font FromFamilyName(
        string familyName,
        float size = 12,
        float scaleX = 1,
        float skewX = 0,
        int weight = FontWeights.Normal,
        int width = FontWidths.Normal,
        FontStyleSlant slant = FontStyleSlant.Upright)
        => new FamilyNameFont(familyName, size, scaleX, skewX, weight, width, slant);

    public static Font FromFile(
        string filePath,
        int index = 0,
        float size = 12,
        float scaleX = 1,
        float skewX = 0,
        int weight = FontWeights.Normal,
        int width = FontWidths.Normal,
        FontStyleSlant slant = FontStyleSlant.Upright)
        => new FilePathFont(filePath, index, size, scaleX, skewX, weight, width, slant);

    public static Font FromStream(
        Stream stream,
        int index = 0,
        bool leaveOpen = false,
        float size = 12,
        float scaleX = 1,
        float skewX = 0,
        int weight = FontWeights.Normal,
        int width = FontWidths.Normal,
        FontStyleSlant slant = FontStyleSlant.Upright)
        => new StreamFont(stream, index, leaveOpen, size, scaleX, skewX, weight, width, slant);

    protected virtual void Dispose(bool disposing)
    {
        Disposed?.Invoke();
        _disposedValue = true;
    }

    ~Font()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        if (!_disposedValue)
            Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}