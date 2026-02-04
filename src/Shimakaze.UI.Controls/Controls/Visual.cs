using System.Drawing;

using CommunityToolkit.Mvvm.ComponentModel;

using SkiaSharp;

namespace Shimakaze.UI.Controls;

public abstract partial class Visual : ObservableObject
{
    /// <summary>
    /// 父元素。
    /// </summary>
    public Visual? Parent { get; internal set; }

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
    public partial Size DesiredSize { get; private set; }

    /// <summary>
    /// 此元素在 Arrange 后的实际渲染边界。
    /// </summary>
    [ObservableProperty]
    public partial Rectangle RenderBounds { get; private set; }

    /// <summary>
    /// 元素的宽度。
    /// </summary>
    [ObservableProperty]
    public partial double? Width { get; set; }

    /// <summary>
    /// 元素的高度。
    /// </summary>
    [ObservableProperty]
    public partial double? Height { get; set; }

    /// <summary>
    /// 元素的最小宽度。
    /// </summary>
    [ObservableProperty]
    public partial double MinWidth { get; set; }

    /// <summary>
    /// 元素的最大宽度。
    /// </summary>
    [ObservableProperty]
    public partial double MaxWidth { get; set; } = double.PositiveInfinity;

    /// <summary>
    /// 元素的最小高度。
    /// </summary>
    [ObservableProperty]
    public partial double MinHeight { get; set; }

    /// <summary>
    /// 元素的最大高度。
    /// </summary>
    [ObservableProperty]
    public partial double MaxHeight { get; set; } = double.PositiveInfinity;

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
    protected double GetConstrainedWidth(double width)
    {
        var w = Width ?? width;
        return Math.Max(MinWidth, Math.Min(MaxWidth, w));
    }

    /// <summary>
    /// 获取考虑 Height、MinHeight、MaxHeight 约束后的高度。
    /// </summary>
    protected double GetConstrainedHeight(double height)
    {
        var h = Height ?? height;
        return Math.Max(MinHeight, Math.Min(MaxHeight, h));
    }

    /// <summary>
    /// 测量此元素。
    /// </summary>
    /// <param name="availableSize"></param>
    public virtual void Measure(Size availableSize)
    {
        RequiredMeasure = false;
        DesiredSize = MeasureCore(availableSize);
    }

    /// <summary>
    /// 测量此元素的核心逻辑。
    /// </summary>
    /// <param name="availableSize"></param>
    /// <returns>元素的期望大小</returns>
    protected virtual Size MeasureCore(Size availableSize)
    {
        // 默认实现：使用显式尺寸或可用尺寸，应用约束并考虑边距
        var width = Width ?? availableSize.Width;
        var height = Height ?? availableSize.Height;

        var constrainedWidth = GetConstrainedWidth(width);
        var constrainedHeight = GetConstrainedHeight(height);

        return new Size(
            (int)(constrainedWidth + Margin.Horizontal),
            (int)(constrainedHeight + Margin.Vertical)
        );
    }

    /// <summary>
    /// 排列此元素。
    /// </summary>
    /// <param name="finalRect"></param>
    public virtual void Arrange(Rectangle finalRect)
    {
        RequiredArrange = false;
        RenderBounds = finalRect;
        ArrangeCore(finalRect);
    }

    /// <summary>
    /// 排列此元素的核心逻辑。
    /// </summary>
    /// <param name="finalRect"></param>
    protected virtual void ArrangeCore(Rectangle finalRect)
    {
        // 默认实现：不需要额外逻辑，RenderBounds 已在 Arrange 中设置
    }

    /// <summary>
    /// 渲染此元素。
    /// </summary>
    /// <param name="canvas"></param>
    public abstract void Render(SKCanvas canvas);
}