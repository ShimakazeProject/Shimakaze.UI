using SkiaSharp;

namespace Shimakaze.UI.Media;

public abstract class ImageSource
{
    protected internal abstract SKImage GetImage();
}