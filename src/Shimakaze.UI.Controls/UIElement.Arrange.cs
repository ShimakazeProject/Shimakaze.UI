
using System.Drawing;

namespace Shimakaze.UI;

public partial class UIElement
{
    /// <summary>
    /// 获取元素最终确定的渲染大小（排列阶段的结果，不含 Margin）。
    /// 在 Measure 之后，Arrange 之前，此值可能不准确。
    /// </summary>
    public SizeF RenderSize { get; protected set; }

    /// <summary>
    /// 获取元素最终的渲染矩形（相对于父容器，包含位置信息，不含 Margin 偏移）。
    /// </summary>
    public RectangleF RenderRect { get; protected set; }

    public bool IsArrangeValid { get; protected set; }

    /// <summary>
    /// 标记排列失效。通常由测量完成但排列未执行，或对齐方式改变触发。
    /// </summary>
    protected void InvalidateArrange()
    {
        if (!IsArrangeValid)
            return;

        IsArrangeValid = false;
    }

    /// <summary>
    /// 公共排列入口。
    /// 负责状态管理、Margin 处理以及调用核心排列逻辑。
    /// </summary>
    /// <param name="finalRect">
    /// 父容器分配给此元素的最终矩形区域。
    /// 注意：这个矩形通常已经包含了 Margin 的空间（即父容器预留了 Margin 位置）。
    /// 元素实际可渲染的内容区域 = finalRect 减去 Margin。
    /// </param>
    public void Arrange(RectangleF finalRect)
    {
        // 如果不可见，设为 Empty 并标记有效
        if (Visibility != Visibility.Visible)
        {
            RenderRect = RectangleF.Empty;
            RenderSize = SizeF.Empty;
            IsArrangeValid = true;
            return;
        }

        // --- 关键逻辑：计算内容可用区域 ---
        // 父容器传来的 finalRect 通常是 "DesiredSize + Margin" 的总空间。
        // 元素实际绘制内容的区域需要扣除 Margin。
        float contentX = finalRect.X + Margin.Left;
        float contentY = finalRect.Y + Margin.Top;
        float contentW = float.Max(0, finalRect.Width - Margin.Horizontal);
        float contentH = float.Max(0, finalRect.Height - Margin.Vertical);

        // 准备传递给 Core 的矩形（内容区域）
        var contentRect = new RectangleF(contentX, contentY, contentW, contentH);

        // 调用核心排列逻辑
        // ArrangeCore 会修改 contentRect 以确认最终使用的尺寸（可能小于分配的尺寸）
        ArrangeCore(ref contentRect);

        // 保存结果
        // RenderRect 存储的是相对于父容器的最终内容矩形（已扣除 Margin 偏移）
        RenderRect = contentRect;
        RenderSize = contentRect.Size;

        // 标记排列已完成且有效
        IsArrangeValid = true;
    }

    /// <summary>
    /// 核心排列逻辑。
    /// 负责根据对齐方式计算最终的渲染矩形，并安排子元素（如果有）。
    /// </summary>
    /// <param name="finalRect">
    /// [In] 父容器分配的内容区域（已扣除 Margin）。
    /// [Out] 元素实际占用的内容区域（可能因对齐而缩小或偏移）。
    /// </param>
    protected virtual void ArrangeCore(ref RectangleF finalRect)
    {
        // 1. 基础数据准备
        float availableWidth = finalRect.Width;
        float availableHeight = finalRect.Height;
        
        // 获取测量阶段计算出的期望大小
        float desiredWidth = DesiredSize.Width;
        float desiredHeight = DesiredSize.Height;

        // 2. 计算最终使用的宽度 (Final Width)
        float finalWidth;
        if (HorizontalAlignment == HorizontalAlignment.Stretch)
        {
            // 拉伸模式：填满可用空间，但不超过最大限制（通常可用空间已经由父级约束过）
            // 注意：如果可用空间是无限大（Infinity），则回退到 DesiredSize
            finalWidth = float.IsInfinity(availableWidth) ? desiredWidth : availableWidth;
        }
        else
        {
            // 非拉伸模式 (Left/Center/Right)：使用期望宽度，但不能超过可用空间
            finalWidth = float.Min(desiredWidth, availableWidth);
        }

        // 3. 计算最终使用的高度 (Final Height)
        float finalHeight;
        if (VerticalAlignment == VerticalAlignment.Stretch)
        {
            finalHeight = float.IsInfinity(availableHeight) ? desiredHeight : availableHeight;
        }
        else
        {
            finalHeight = float.Min(desiredHeight, availableHeight);
        }

        // 4. 防止负数
        finalWidth = float.Max(0, finalWidth);
        finalHeight = float.Max(0, finalHeight);

        // 5. 计算起始位置 (X, Y) 基于对齐方式
        float finalX = finalRect.X;
        float finalY = finalRect.Y;

        // 水平对齐偏移
        if (HorizontalAlignment == HorizontalAlignment.Center)
        {
            float extraSpace = availableWidth - finalWidth;
            if (extraSpace > 0)
                finalX += extraSpace / 2;
        }
        else if (HorizontalAlignment == HorizontalAlignment.Right)
        {
            float extraSpace = availableWidth - finalWidth;
            if (extraSpace > 0)
                finalX += extraSpace;
        }
        // Left 不需要额外计算，默认为 finalRect.X

        // 垂直对齐偏移
        if (VerticalAlignment == VerticalAlignment.Center)
        {
            float extraSpace = availableHeight - finalHeight;
            if (extraSpace > 0)
                finalY += extraSpace / 2;
        }
        else if (VerticalAlignment == VerticalAlignment.Bottom)
        {
            float extraSpace = availableHeight - finalHeight;
            if (extraSpace > 0)
                finalY += extraSpace;
        }
        // Top 不需要额外计算

        // 6. 更新 ref 参数，返回最终确定的矩形
        // 这就是子类（如 Panel）在排列子元素时应该依赖的边界，
        // 也是绘制系统最终用来裁剪绘制的边界。
        finalRect = new RectangleF(finalX, finalY, finalWidth, finalHeight);

        // 7. 【扩展点】如果有子元素，子类应在此处遍历并调用 child.Arrange()
        // 例如：
        // if (this is Panel panel)
        // {
        //     foreach (var child in panel.Children)
        //     {
        //         var childRect = CalculateChildRect(child, finalRect);
        //         child.Arrange(childRect);
        //     }
        // }
    }
    
    /// <summary>
    /// 获取元素的渲染边界（包含 Margin 的外部边界）。
    /// 基于最新的 Arrange 结果计算。
    /// </summary>
    public override RectangleF RenderBounds
    {
        get
        {
            if (Visibility != Visibility.Visible)
                return RectangleF.Empty;

            if (!IsArrangeValid)
            {
                // 如果还没 Arrange，返回一个基于 DesiredSize 的估算值（ fallback ）
                // 或者返回 Empty，取决于你的框架策略
                return new RectangleF(0, 0, DesiredSize.Width + Margin.Horizontal, DesiredSize.Height + Margin.Vertical);
            }

            // 真正的 RenderBounds = 内容区域 (RenderRect) + Margin 的外扩
            // RenderRect 已经是扣除了 Left/Top 偏移后的，所以要还原回去计算外边界
            return new RectangleF(
                RenderRect.X - Margin.Left,
                RenderRect.Y - Margin.Top,
                RenderRect.Width + Margin.Horizontal,
                RenderRect.Height + Margin.Vertical
            );
        }
    }
}