using System.Drawing;


using Shimakaze.UI.Fonts;
using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Controls;

/// <summary>
/// 用于显示文本的轻量级元素。
/// </summary>
public partial class TextBlock : UIElement
{
    [ObservableProperty]
    public partial string? Text { get; set; }

    [ObservableProperty]
    public partial Font? Font { get; set; }

    [ObservableProperty]
    public partial TextAlign TextAlign { get; set; } = TextAlign.Left;

    protected override SizeF MeasureCore(SizeF availableSize)
    {
        var skfont = FontManager.GetFont(Font);

        skfont.MeasureText(Text, out var rect);

        var width = Width ?? rect.Width;
        var height = Height ?? rect.Height;

        var constrainedWidth = GetConstrainedWidth(width);
        var constrainedHeight = GetConstrainedHeight(height);

        return new(constrainedWidth, constrainedHeight);
    }

    public override void OnRender(Renderer renderer)
    {
        if (Text is null)
            return;

        renderer.DrawText(Text, RenderBounds.Location, TextAlign, Font);
    }

}