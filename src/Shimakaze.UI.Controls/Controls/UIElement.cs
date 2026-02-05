using System.Drawing;

using Shimakaze.UI.Bindings;
using Shimakaze.UI.Core;
using Shimakaze.UI.Input.EventArgs;

namespace Shimakaze.UI.Controls;

public abstract partial class UIElement : Visual
{
    /// <summary>
    /// 是否拥有焦点。
    /// </summary>
    [ObservableProperty]
    public partial bool IsFocused { get; private set; }

    /// <summary>
    /// 是否启用。
    /// </summary>
    [ObservableProperty]
    public partial bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 附加的任意数据。
    /// </summary>
    [ObservableProperty]
    public partial object? Tag { get; set; }

    /// <summary>
    /// 元素加载事件。
    /// </summary>
    public event UIEventHandler<UIElement, EventArgs>? Loaded;

    /// <summary>
    /// 元素卸载事件。
    /// </summary>
    public event UIEventHandler<UIElement, EventArgs>? Unloaded;

    /// <summary>
    /// 鼠标按下事件。
    /// </summary>
    public event UIEventHandler<UIElement, MouseButtonEventArgs>? MouseDown;

    /// <summary>
    /// 鼠标释放事件。
    /// </summary>
    public event UIEventHandler<UIElement, MouseButtonEventArgs>? MouseUp;

    /// <summary>
    /// 鼠标点击事件。
    /// </summary>
    public event UIEventHandler<UIElement, MouseClickEventArgs>? MouseClick;

    /// <summary>
    /// 鼠标双击事件。
    /// </summary>
    public event UIEventHandler<UIElement, MouseClickEventArgs>? MouseDoubleClick;

    /// <summary>
    /// 鼠标移动事件。
    /// </summary>
    public event UIEventHandler<UIElement, MouseMoveEventArgs>? MouseMove;

    /// <summary>
    /// 鼠标滚轮事件。
    /// </summary>
    public event UIEventHandler<UIElement, MouseScrollEventArgs>? MouseScroll;

    /// <summary>
    /// 键盘按键按下事件。
    /// </summary>
    public event UIEventHandler<UIElement, KeyboardKeyEventArgs>? KeyDown;

    /// <summary>
    /// 键盘按键释放事件。
    /// </summary>
    public event UIEventHandler<UIElement, KeyboardKeyEventArgs>? KeyUp;

    /// <summary>
    /// 控件获得焦点事件。
    /// </summary>
    public event UIEventHandler<UIElement, EventArgs>? GotFocus;

    /// <summary>
    /// 控件失去焦点事件。
    /// </summary>
    public event UIEventHandler<UIElement, EventArgs>? LostFocus;

    /// <summary>
    /// 判断给定的点是否在此元素的渲染区域内。
    /// </summary>
    /// <param name="point">测试点（屏幕坐标）</param>
    /// <returns>如果点在元素内则返回 true，否则返回 false</returns>
    public virtual bool HitTest(PointF point)
        => IsEnabled && RenderBounds.Contains(point);

    /// <summary>
    /// 在指定点执行命中测试，返回最底层的命中元素。
    /// </summary>
    /// <param name="point">测试点</param>
    /// <returns>命中的元素，如果没有命中则返回 null</returns>
    public virtual UIElement? HitTestElement(PointF point)
        => HitTest(point) ? this : null;

    /// <summary>
    /// 触发 Loaded 事件。
    /// </summary>
    protected virtual void OnLoaded()
        => Loaded?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// 触发 Unloaded 事件。
    /// </summary>
    protected virtual void OnUnloaded()
        => Unloaded?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// 触发 MouseDown 事件。
    /// </summary>
    protected virtual void OnMouseDown(MouseButtonEventArgs e)
        => MouseDown?.Invoke(this, e);

    /// <summary>
    /// 触发 MouseUp 事件。
    /// </summary>
    protected virtual void OnMouseUp(MouseButtonEventArgs e)
        => MouseUp?.Invoke(this, e);

    /// <summary>
    /// 触发 MouseClick 事件。
    /// </summary>
    protected virtual void OnMouseClick(MouseClickEventArgs e)
        => MouseClick?.Invoke(this, e);

    /// <summary>
    /// 触发 MouseDoubleClick 事件。
    /// </summary>
    protected virtual void OnMouseDoubleClick(MouseClickEventArgs e)
        => MouseDoubleClick?.Invoke(this, e);

    /// <summary>
    /// 触发 MouseMove 事件。
    /// </summary>
    protected virtual void OnMouseMove(MouseMoveEventArgs e)
        => MouseMove?.Invoke(this, e);

    /// <summary>
    /// 触发 MouseScroll 事件。
    /// </summary>
    protected virtual void OnMouseScroll(MouseScrollEventArgs e)
        => MouseScroll?.Invoke(this, e);

    /// <summary>
    /// 触发 KeyDown 事件。
    /// </summary>
    protected virtual void OnKeyDown(KeyboardKeyEventArgs e)
        => KeyDown?.Invoke(this, e);

    /// <summary>
    /// 触发 KeyUp 事件。
    /// </summary>
    protected virtual void OnKeyUp(KeyboardKeyEventArgs e)
        => KeyUp?.Invoke(this, e);


    /// <summary>
    /// 请求焦点到此控件。
    /// </summary>
    /// <returns>如果成功获得焦点则返回 true，否则返回 false</returns>
    public virtual bool Focus()
    {
        if (!IsFocused || !IsEnabled || Visiblity is not Visiblity.Visible)
            return false;

        IsFocused = true;
        OnGotFocus();
        return true;
    }

    /// <summary>
    /// 释放焦点。
    /// </summary>
    public virtual void Unfocus()
    {
        if (!IsFocused)
            return;

        IsFocused = false;
        OnLostFocus();
    }

    /// <summary>
    /// 触发 GotFocus 事件。
    /// </summary>
    protected virtual void OnGotFocus()
    {
        GotFocus?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 触发 LostFocus 事件。
    /// </summary>
    protected virtual void OnLostFocus()
    {
        LostFocus?.Invoke(this, EventArgs.Empty);
    }
}