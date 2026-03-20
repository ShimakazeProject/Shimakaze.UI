using Shimakaze.UI.Fonts;
using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Controls;

public class TextBlock : UIElement
{
    public string? Text
    {
        get { return (string?)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextBlock), new PropertyMetadata());

    public TextAlign TextAlign
    {
        get { return (TextAlign)GetValue(TextAlignProperty)!; }
        set { SetValue(TextAlignProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TextAlign.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TextAlignProperty =
        DependencyProperty.Register(nameof(TextAlign), typeof(TextAlign), typeof(TextBlock), new PropertyMetadata(TextAlign.Left));

    public Font? Font
    {
        get { return (Font?)GetValue(FontProperty); }
        set { SetValue(FontProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Font.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FontProperty =
        DependencyProperty.Register(nameof(Font), typeof(Font), typeof(TextBlock), new PropertyMetadata());

    protected internal override void OnRender(Renderer renderer, double deltaTime)
    {
        base.OnRender(renderer, deltaTime);
        if (!string.IsNullOrWhiteSpace(Text))
            renderer.DrawText(Text, RenderRect, TextAlign, Font);
    }
}