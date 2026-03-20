using SkiaSharp;

namespace Shimakaze.UI.Media;

public sealed class SkiaImageSource(SKImage image, bool leaveOpen = false) : ImageSource, IDisposable
{
    private readonly SKBitmap? _bitmap;
    private bool _disposedValue;

    public SkiaImageSource(SKBitmap bitmap)
        : this(SKImage.FromBitmap(bitmap))
    {
        // 保持引用 避免被 GC 回收
        _bitmap = bitmap;
    }

    protected internal override SKImage GetImage() => image;

    private void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            if (!leaveOpen)
            {
                image.Dispose();
                _bitmap?.Dispose();
            }
        }

        _disposedValue = true;
    }
    public override void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}