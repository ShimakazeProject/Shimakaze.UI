using System.Drawing;

using Shimakaze.UI.Media;
using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Controls;

public partial class Border : FrameworkElement
{
    [ObservableProperty]
    public partial Brush? BorderBrush { get; set; }

    [ObservableProperty]
    public partial Thickness Thickness { get; set; } = default;

    public override void OnRender(Renderer renderer)
    {
        base.OnRender(renderer);

        if (BorderBrush is not null)
        {
            RectangleF left = new(
                RenderBounds.Left,
                RenderBounds.Top,
                Thickness.Left,
                RenderBounds.Height);
            RectangleF top = new(
                RenderBounds.Left + Thickness.Left,
                RenderBounds.Top,
                RenderBounds.Width - Thickness.Left * 2,
                Thickness.Top);
            RectangleF right = left with
            {
                X = RenderBounds.Right - Thickness.Right,
                Width = Thickness.Right,
            };
            RectangleF bottom = top with
            {
                Y = RenderBounds.Bottom - Thickness.Bottom,
                Height = Thickness.Bottom,
            };
            BorderBrush.OnRender(renderer, left, top, right, bottom);
        }
    }
}
