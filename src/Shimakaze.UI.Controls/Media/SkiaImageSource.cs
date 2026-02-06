using SkiaSharp;

namespace Shimakaze.UI.Media;

public sealed class SkiaImageSource(SKImage image) : ImageSource
{
    private readonly SKBitmap? _bitmap;

    public SkiaImageSource(SKBitmap bitmap)
        : this(SKImage.FromBitmap(bitmap))
    {
        // 保持引用 避免被 GC 回收
        _bitmap = bitmap;
    }

    protected internal override SKImage GetImage() => image;
}