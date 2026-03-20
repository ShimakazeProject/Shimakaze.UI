using System.Drawing;

using Shimakaze.UI.Rendering;

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

    /// <summary>
    /// 获取或设置元素的宽度。
    /// </summary>
    public UIElement? Content
    {
        get => (UIElement?)GetValue(ContentProperty)!;
        set => SetValue(ContentProperty, value);
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
}