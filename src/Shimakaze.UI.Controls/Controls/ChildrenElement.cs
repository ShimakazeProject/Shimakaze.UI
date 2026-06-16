using System.Drawing;

using Shimakaze.Foundation.Rendering;

namespace Shimakaze.UI.Controls;

public class ChildrenElement : UIElement
{
    public ChildrenElement()
    {
        Children = new(this);
    }

    public ChildrenCollection Children { get; }

    protected internal override void OnInitialize()
    {
        base.OnInitialize();
        foreach (var item in Children)
            item?.OnInitialize();
    }

    protected internal override void OnUpdate(double deltaTime)
    {
        base.OnUpdate(deltaTime);
        foreach (var item in Children)
            item?.OnUpdate(deltaTime);
    }

    protected internal override void OnRender(Renderer renderer, double deltaTime)
    {
        base.OnRender(renderer, deltaTime);

        using var clip = renderer.ClipRect(RenderBounds);
        foreach (var item in Children)
            item?.OnRender(clip, deltaTime);
    }

    protected override void MeasureCore(ref SizeF availableSize)
    {
        base.MeasureCore(ref availableSize);
        foreach (var item in Children)
            item?.Measure(availableSize);
    }

    protected override void ArrangeCore(ref RectangleF finalRect)
    {
        base.ArrangeCore(ref finalRect);
        foreach (var item in Children)
            item?.Arrange(finalRect);
    }

    public override UIElement? HitTestElement(in PointF point)
    {
        foreach (var item in Children)
        {
            if (item?.HitTestElement(point) is { } e)
                return e;
        }

        return base.HitTestElement(point);
    }
}