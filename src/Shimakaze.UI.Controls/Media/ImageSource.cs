using SkiaSharp;

namespace Shimakaze.UI.Media;

public abstract class ImageSource : IDisposable
{
    public abstract void Dispose();
    protected internal abstract SKImage GetImage();
}