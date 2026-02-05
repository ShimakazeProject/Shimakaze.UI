using System.Drawing;

using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Controls;

public abstract partial class Visual : BindableObject
{
    /// <summary>
    /// 父元素。
    /// </summary>
    public Visual? Parent { get; internal set; }

    /// <summary>
    /// 是否可见。
    /// </summary>
    [ObservableProperty]
    public partial Visiblity Visiblity { get; set; } = Visiblity.Visible;

    /// <summary>
    /// 透明度（0-1）。
    /// </summary>
    [ObservableProperty]
    public partial float Opacity { get; set; } = 1.0f;

    /// <summary>
    /// 是否需要重新测量。
    /// </summary>
    [ObservableProperty]
    public partial bool RequiredMeasure { get; private set; }

    /// <summary>
    /// 是否需要重新排列。
    /// </summary>
    [ObservableProperty]
    public partial bool RequiredArrange { get; private set; }

    /// <summary>
    /// 此元素的期望大小。
    /// </summary>
    [ObservableProperty]
    public partial SizeF DesiredSize { get; private set; }

    /// <summary>
    /// 此元素在 Arrange 后的实际渲染边界。
    /// </summary>
    [ObservableProperty]
    public partial RectangleF RenderBounds { get; private set; }

    /// <summary>
    /// 元素的宽度。
    /// </summary>
    [ObservableProperty]
    public partial float? Width { get; set; }

    /// <summary>
    /// 元素的高度。
    /// </summary>
    [ObservableProperty]
    public partial float? Height { get; set; }

    /// <summary>
    /// 元素的最小宽度。
    /// </summary>
    [ObservableProperty]
    public partial float MinWidth { get; set; }

    /// <summary>
    /// 元素的最大宽度。
    /// </summary>
    [ObservableProperty]
    public partial float MaxWidth { get; set; } = float.PositiveInfinity;

    /// <summary>
    /// 元素的最小高度。
    /// </summary>
    [ObservableProperty]
    public partial float MinHeight { get; set; }

    /// <summary>
    /// 元素的最大高度。
    /// </summary>
    [ObservableProperty]
    public partial float MaxHeight { get; set; } = float.PositiveInfinity;

    /// <summary>
    /// 元素的外边距。
    /// </summary>
    [ObservableProperty]
    public partial Thickness Margin { get; set; }

    /// <summary>
    /// 请求重新测量此元素及其父链。
    /// </summary>
    public virtual void InvalidateMeasure()
    {
        if (RequiredMeasure)
            return;

        RequiredMeasure = true;
        Parent?.InvalidateMeasure();
    }

    /// <summary>
    /// 请求重新排列此元素。
    /// </summary>
    public virtual void InvalidateArrange()
    {
        if (RequiredArrange)
            return;

        RequiredArrange = true;
        Parent?.InvalidateArrange();
    }

    /// <summary>
    /// 获取考虑 Width、MinWidth、MaxWidth 约束后的宽度。
    /// </summary>
    protected float GetConstrainedWidth(float width)
    {
        var w = Width ?? width;
        return Math.Max(MinWidth, Math.Min(MaxWidth, w));
    }

    /// <summary>
    /// 获取考虑 Height、MinHeight、MaxHeight 约束后的高度。
    /// </summary>
    protected float GetConstrainedHeight(float height)
    {
        var h = Height ?? height;
        return Math.Max(MinHeight, Math.Min(MaxHeight, h));
    }

    /// <summary>
    /// 测量此元素。
    /// </summary>
    /// <param name="availableSize"></param>
    public virtual void Measure(SizeF availableSize)
    {
        RequiredMeasure = false;
        DesiredSize = MeasureCore(availableSize);
    }

    /// <summary>
    /// 测量此元素的核心逻辑。
    /// </summary>
    /// <param name="availableSize"></param>
    /// <returns>元素的期望大小</returns>
    protected virtual SizeF MeasureCore(SizeF availableSize)
    {
        // 默认实现：使用显式尺寸或可用尺寸，应用约束
        var width = Width ?? availableSize.Width;
        var height = Height ?? availableSize.Height;

        var constrainedWidth = GetConstrainedWidth(width);
        var constrainedHeight = GetConstrainedHeight(height);

        return new(constrainedWidth, constrainedHeight);
    }

    /// <summary>
    /// 排列此元素。
    /// </summary>
    /// <param name="finalRect"></param>
    public virtual void Arrange(RectangleF finalRect)
    {
        RequiredArrange = false;
        RenderBounds = ArrangeCore(finalRect);
    }

    /// <summary>
    /// 排列此元素的核心逻辑。
    /// </summary>
    /// <param name="finalRect"></param>
    protected virtual RectangleF ArrangeCore(RectangleF finalRect)
    {
        return finalRect;
    }

    /// <summary>
    /// 渲染此元素。
    /// </summary>
    /// <param name="renderer"></param>
    public abstract void Render(Renderer renderer);
}