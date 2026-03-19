
using System.Drawing;

namespace Shimakaze.UI;

public partial class UIElement
{
    /// <summary>
    /// 获取元素的期望大小（测量阶段的结果，不含 Margin）。
    /// </summary>
    public SizeF DesiredSize { get; protected set; }

    public bool IsMeasureValid { get; protected set; }

    /// <summary>
    /// 公共测量入口。
    /// 负责状态管理和调用核心测量逻辑。
    /// </summary>
    public void Measure(SizeF availableSize)
    {
        // 如果不可见，直接设为 Empty 并标记有效，跳过核心计算
        if (Visibility != Visibility.Visible)
        {
            DesiredSize = SizeF.Empty;
            IsMeasureValid = true;
            return;
        }

        // 调用核心测量逻辑 (由子类重写或基类提供默认实现)
        // MeasureCore 返回计算出的 DesiredSize
        MeasureCore(ref availableSize);
        DesiredSize = availableSize;

        // 标记测量已完成且有效
        IsMeasureValid = true;
    }

    /// <summary>
    /// 核心测量逻辑。
    /// 子类应重写此方法来提供具体的布局行为。
    /// </summary>
    /// <param name="availableSize">父元素提供的可用空间</param>
    /// <returns>元素期望的大小 (DesiredSize)</returns>
    protected virtual void MeasureCore(ref SizeF availableSize)
    {
        float desiredWidth;
        float desiredHeight;

        // --- 1. 计算宽度 ---
        if (!float.IsNaN(Width))
        {
            // 情况 A: 用户显式设置了宽度
            desiredWidth = Width;
        }
        else
        {
            // 情况 B: 自适应宽度 (Auto)
            // 如果可用空间是无限大 (Infinity)，对于没有内容的基类元素，其自然大小通常为 0 或 MinWidth
            // 如果有内容，子类会在这里测量内容并返回累加值
            float availableW = float.IsInfinity(availableSize.Width) ? 0 : availableSize.Width;

            // 默认策略：尽可能占用可用空间，但不超过最大限制
            desiredWidth = float.Min(availableW, MaxWidth);
        }

        // --- 2. 计算高度 (逻辑同宽度) ---
        if (!float.IsNaN(Height))
        {
            desiredHeight = Height;
        }
        else
        {
            float availableH = float.IsInfinity(availableSize.Height) ? 0 : availableSize.Height;
            desiredHeight = float.Min(availableH, MaxHeight);
        }

        // --- 3. 应用 Min/Max 约束钳制 ---
        // 将 desiredWidth 限制在 [MinWidth, MaxWidth] 区间内
        desiredWidth = float.Clamp(desiredWidth, MinWidth, MaxWidth);

        // 将 desiredHeight 限制在 [MinHeight, MaxHeight] 区间内
        desiredHeight = float.Clamp(desiredHeight, MinHeight, MaxHeight);

        // --- 4. 返回结果 (值类型直接返回) ---
        availableSize = new SizeF(desiredWidth, desiredHeight);
    }
}