using Shimakaze.UI.Media;
using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Controls;

public class Image : UIElement
{
    public ImageSource? ImageSource
    {
        get { return (ImageSource?)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(Image), new PropertyMetadata());

    protected internal override void OnRender(Renderer renderer, double deltaTime)
    {
        base.OnRender(renderer, deltaTime);
        if (ImageSource is not null)
        {
            var image = ImageSource.GetImage();
            renderer.DrawImage(image, RenderBounds);
        }
    }
}