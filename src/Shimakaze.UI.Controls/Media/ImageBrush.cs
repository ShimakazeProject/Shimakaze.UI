using System.Drawing;

using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Media;

public sealed class ImageBrush(ImageSource image) : Brush
{
    public ImageSource Source { get; } = image;
    public Stretch Stretch { get; set; } = Stretch.None;
    protected internal override void OnRender(Renderer renderer, params IEnumerable<RectangleF> bounds)
    {
        var left = bounds.Min(i => i.Left);
        var top = bounds.Min(i => i.Top);
        var right = bounds.Max(i => i.Right);
        var bottom = bounds.Max(i => i.Bottom);
        var rect = RectangleF.FromLTRB(left, top, right, bottom);

        var image = Source.GetImage();
        SizeF source;
        switch (Stretch)
        {
            case Stretch.None:
                source = new()
                {
                    Width = rect.Width,
                    Height = rect.Height,
                };
                break;
            case Stretch.Fill:
                source = new()
                {
                    Width = image.Width,
                    Height = image.Height,
                };
                break;
            case Stretch.Uniform:
                float ratio = Math.Min(rect.Width / image.Width, rect.Height / image.Height);
                source = new()
                {
                    Width = image.Width * ratio,
                    Height = image.Height * ratio,
                };
                break;
            case Stretch.UniformToFill:
                float fillRatio = Math.Max(rect.Width / image.Width, rect.Height / image.Height);
                source = new()
                {
                    Width = image.Width * fillRatio,
                    Height = image.Height * fillRatio,
                };
                break;
            default:
                source = SizeF.Empty;
                break;
        }

        foreach (var bound in bounds)
            renderer.DrawImage(image, source, bound);
    }
}