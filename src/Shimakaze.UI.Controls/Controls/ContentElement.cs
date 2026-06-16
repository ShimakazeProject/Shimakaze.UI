using System.Drawing;

using Shimakaze.Foundation.Rendering;

namespace Shimakaze.UI.Controls;

public class ContentElement : UIElement
{
    /// <summary>
    /// 标识 Content 依赖属性。
    /// </summary>
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(UIElement),
        typeof(ContentElement),
        new PropertyMetadata()
    );

    public UIElement? Content
    {
        get => (UIElement?)GetValue(ContentProperty);
        set
        {
            value?.Parent = this;
            value?.VisualParentChanged += ContentElement_VisualParentChanged;
            SetValue(ContentProperty, value);
        }
    }

    private void ContentElement_VisualParentChanged(Visual sender, EventArgs eventArgs)
    {
        if (sender.Parent == this)
            return;

        sender.VisualParentChanged -= ContentElement_VisualParentChanged;
        if (sender == Content)
            Content = null;
    }

    protected internal override void OnInitialize()
    {
        base.OnInitialize();
        Content?.OnInitialize();
    }

    protected internal override void OnUpdate(double deltaTime)
    {
        base.OnUpdate(deltaTime);
        Content?.OnUpdate(deltaTime);
    }

    protected internal override void OnRender(Renderer renderer, double deltaTime)
    {
        base.OnRender(renderer, deltaTime);

        using var clip = renderer.ClipRect(RenderBounds);
        Content?.OnRender(clip, deltaTime);
    }

    protected override void MeasureCore(ref SizeF availableSize)
    {
        base.MeasureCore(ref availableSize);
        Content?.Measure(availableSize);
    }

    protected override void ArrangeCore(ref RectangleF finalRect)
    {
        base.ArrangeCore(ref finalRect);
        Content?.Arrange(finalRect);
    }

    public override UIElement? HitTestElement(in PointF point)
        => Content?.HitTestElement(point) ?? base.HitTestElement(point);
}