using System.Drawing;

namespace Shimakaze.UI;

/// <summary>
/// 提供渲染支持、命中测试和坐标转换功能的基类。
/// </summary>
public abstract class Visual : DependencyObject
{
    /// <summary>
    /// 获取或设置元素的可见性。
    /// </summary>
    public Visiblity Visiblity
    {
        get => IsVisible ? Visiblity.Visible : Visiblity.Collapsed;
        set => IsVisible = value == Visiblity.Visible;
    }

    /// <summary>
    /// 获取或设置元素的不透明度因子。
    /// </summary>
    /// <remarks>值在 0.0 到 1.0 之间，0.0 表示完全透明，1.0 表示完全不透明。</remarks>
    public float Opacity
    {
        get;
        set => field = Math.Clamp(value, 0.0f, 1.0f);
    } = 1.0f;

    /// <summary>
    /// 获取此元素的父级可视化对象。
    /// </summary>
    public Visual? Parent
    {
        get;
        protected internal set
        {
            if (field == value)
                return;

            field = value;
            OnVisualParentChanged();
        }
    }

    /// <summary>
    /// 获取此元素的渲染边界。
    /// </summary>
    public virtual RectangleF RenderBounds => RectangleF.Empty;

    /// <summary>
    /// 获取一个值，该值指示此元素在用户界面 (UI) 中是否可见。
    /// </summary>
    public bool IsVisible { get; private set; } = true;

    #region 坐标转换

    /// <summary>
    /// 将点从此元素的坐标空间转换到指定元素的坐标空间。
    /// </summary>
    /// <param name="point">要转换的点</param>
    /// <param name="destination">目标元素</param>
    /// <returns>转换后的点</returns>
    public virtual Point TransformToVisual(Visual? destination)
    {
        if (destination == this)
            return Point.Empty;

        if (destination == null)
        {
            // 转换到屏幕坐标
            var point = GetOffset();
            var current1 = this;
            while (current1.Parent != null)
            {
                current1 = current1.Parent;
                point.Offset(current1.GetOffset());
            }
            return point;
        }

        // 转换到另一个元素
        var thisPoint = GetOffset();
        var destPoint = Point.Empty;

        // 向上找到公共祖先
        var thisAncestors = new List<Point> { thisPoint };
        var current = this;
        while (current.Parent != null)
        {
            current = current.Parent;
            thisPoint = current.GetOffset();
            thisPoint.Offset(thisAncestors[^1]);
            thisAncestors.Add(thisPoint);
        }

        var destAncestors = new List<Point> { destPoint };
        current = destination;
        while (current.Parent != null)
        {
            current = current.Parent;
            destPoint = current.GetOffset();
            destPoint.Offset(destAncestors[^1]);
            destAncestors.Add(destPoint);
        }

        // 计算相对位置
        var thisOffset = thisAncestors[^1];
        var destOffset = destAncestors[^1];
        return new Point(thisOffset.X - destOffset.X, thisOffset.Y - destOffset.Y);
    }

    /// <summary>
    /// 将矩形从此元素的坐标空间转换到指定元素的坐标空间。
    /// </summary>
    /// <param name="rectangle">要转换的矩形</param>
    /// <param name="destination">目标元素</param>
    /// <returns>转换后的矩形</returns>
    public RectangleF TransformToVisual(RectangleF rectangle, Visual? destination)
    {
        var point = TransformToVisual(destination);
        return new RectangleF(
            rectangle.X + point.X,
            rectangle.Y + point.Y,
            rectangle.Width,
            rectangle.Height
        );
    }

    /// <summary>
    /// 获取此元素相对于其父级的偏移量。
    /// </summary>
    /// <returns>偏移量</returns>
    protected virtual Point GetOffset() => Point.Empty;

    #endregion

    #region 命中测试

    /// <summary>
    /// 确定给定的点是否在此元素的渲染区域内。
    /// </summary>
    /// <param name="point">测试点</param>
    /// <returns>如果点在元素内则返回 true，否则返回 false</returns>
    public virtual bool HitTest(PointF point) => IsVisible && RenderBounds.Contains(point);

    #endregion

    #region 虚方法

    /// <summary>
    /// 当父级可视化对象更改时调用。
    /// </summary>
    protected virtual void OnVisualParentChanged()
    {
    }

    /// <summary>
    /// 当元素需要重新渲染时调用。
    /// </summary>
    protected virtual void InvalidateVisual()
    {
    }

    #endregion
}