using Shimakaze.UI.Media;
using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Controls;

public abstract partial class FrameworkElement : UIElement
{
    [ObservableProperty]
    public partial Brush? Background { get; set; }

    public override void OnRender(Renderer renderer)
    {
        Background?.OnRender(renderer, RenderBounds);
    }
}
