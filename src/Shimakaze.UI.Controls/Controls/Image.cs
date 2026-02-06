
using System.Drawing;

using Shimakaze.UI.Rendering;

using SkiaSharp;

namespace Shimakaze.UI.Controls;

public partial class Image : UIElement
{
    [ObservableProperty]
    public partial SKImage? Source { get; set; }

    protected override SizeF MeasureCore(SizeF availableSize)
    {
        var width = Width ?? Source?.Width ?? availableSize.Width;
        var height = Height ?? Source?.Height ?? availableSize.Height;

        var constrainedWidth = GetConstrainedWidth(width);
        var constrainedHeight = GetConstrainedHeight(height);

        return new(constrainedWidth, constrainedHeight);
    }

    public override void OnRender(Renderer renderer)
    {
        if (Source is null)
            return;

        renderer.DrawImage(Source, RenderBounds);
    }
}