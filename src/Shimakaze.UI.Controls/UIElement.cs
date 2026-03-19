using System.Drawing;

using Shimakaze.UI.Core;
using Shimakaze.UI.Rendering;

namespace Shimakaze.UI;

/// <summary>
/// 表示用户界面 (UI) 元素的基类，提供交互功能和路由事件支持。
/// </summary>
public partial class UIElement : Visual
{

    #region 路由事件 - 冒泡事件

    /// <summary>
    /// 标识 MouseDown 路由事件。
    /// </summary>
    public static readonly RoutedEvent MouseDownEvent = EventManager.RegisterRoutedEvent(
        nameof(MouseDown),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 MouseUp 路由事件。
    /// </summary>
    public static readonly RoutedEvent MouseUpEvent = EventManager.RegisterRoutedEvent(
        nameof(MouseUp),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 MouseMove 路由事件。
    /// </summary>
    public static readonly RoutedEvent MouseMoveEvent = EventManager.RegisterRoutedEvent(
        nameof(MouseMove),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 Click 路由事件。
    /// </summary>
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
        nameof(Click),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 GotFocus 路由事件。
    /// </summary>
    public static readonly RoutedEvent GotFocusEvent = EventManager.RegisterRoutedEvent(
        nameof(GotFocus),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 LostFocus 路由事件。
    /// </summary>
    public static readonly RoutedEvent LostFocusEvent = EventManager.RegisterRoutedEvent(
        nameof(LostFocus),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    #endregion

    #region 路由事件 - 隧道事件

    /// <summary>
    /// 标识 PreviewMouseDown 路由事件（隧道）。
    /// </summary>
    public static readonly RoutedEvent PreviewMouseDownEvent = EventManager.RegisterRoutedEvent(
        nameof(PreviewMouseDown),
        RoutingStrategy.Tunnel,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 PreviewMouseUp 路由事件（隧道）。
    /// </summary>
    public static readonly RoutedEvent PreviewMouseUpEvent = EventManager.RegisterRoutedEvent(
        nameof(PreviewMouseUp),
        RoutingStrategy.Tunnel,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 PreviewMouseMove 路由事件（隧道）。
    /// </summary>
    public static readonly RoutedEvent PreviewMouseMoveEvent = EventManager.RegisterRoutedEvent(
        nameof(PreviewMouseMove),
        RoutingStrategy.Tunnel,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 PreviewClick 路由事件（隧道）。
    /// </summary>
    public static readonly RoutedEvent PreviewClickEvent = EventManager.RegisterRoutedEvent(
        nameof(PreviewClick),
        RoutingStrategy.Tunnel,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 PreviewGotFocus 路由事件（隧道）。
    /// </summary>
    public static readonly RoutedEvent PreviewGotFocusEvent = EventManager.RegisterRoutedEvent(
        nameof(PreviewGotFocus),
        RoutingStrategy.Tunnel,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    /// <summary>
    /// 标识 PreviewLostFocus 路由事件（隧道）。
    /// </summary>
    public static readonly RoutedEvent PreviewLostFocusEvent = EventManager.RegisterRoutedEvent(
        nameof(PreviewLostFocus),
        RoutingStrategy.Tunnel,
        typeof(RoutedEventHandler),
        typeof(UIElement)
    );

    #endregion


    #region 事件 - 冒泡事件

    /// <summary>
    /// 鼠标按下事件（冒泡）。
    /// </summary>
    public event RoutedEventHandler? MouseDown
    {
        add => AddHandler(MouseDownEvent, value);
        remove => RemoveHandler(MouseDownEvent, value);
    }

    /// <summary>
    /// 鼠标释放事件（冒泡）。
    /// </summary>
    public event RoutedEventHandler? MouseUp
    {
        add => AddHandler(MouseUpEvent, value);
        remove => RemoveHandler(MouseUpEvent, value);
    }

    /// <summary>
    /// 鼠标移动事件（冒泡）。
    /// </summary>
    public event RoutedEventHandler? MouseMove
    {
        add => AddHandler(MouseMoveEvent, value);
        remove => RemoveHandler(MouseMoveEvent, value);
    }

    /// <summary>
    /// 点击事件（冒泡）。
    /// </summary>
    public event RoutedEventHandler? Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    /// <summary>
    /// 获得焦点事件（冒泡）。
    /// </summary>
    public event RoutedEventHandler? GotFocus
    {
        add => AddHandler(GotFocusEvent, value);
        remove => RemoveHandler(GotFocusEvent, value);
    }

    /// <summary>
    /// 失去焦点事件（冒泡）。
    /// </summary>
    public event RoutedEventHandler? LostFocus
    {
        add => AddHandler(LostFocusEvent, value);
        remove => RemoveHandler(LostFocusEvent, value);
    }

    #endregion

    #region 事件 - 隧道事件

    /// <summary>
    /// 鼠标按下预览事件（隧道）。
    /// </summary>
    public event RoutedEventHandler? PreviewMouseDown
    {
        add => AddHandler(PreviewMouseDownEvent, value);
        remove => RemoveHandler(PreviewMouseDownEvent, value);
    }

    /// <summary>
    /// 鼠标释放预览事件（隧道）。
    /// </summary>
    public event RoutedEventHandler? PreviewMouseUp
    {
        add => AddHandler(PreviewMouseUpEvent, value);
        remove => RemoveHandler(PreviewMouseUpEvent, value);
    }

    /// <summary>
    /// 鼠标移动预览事件（隧道）。
    /// </summary>
    public event RoutedEventHandler? PreviewMouseMove
    {
        add => AddHandler(PreviewMouseMoveEvent, value);
        remove => RemoveHandler(PreviewMouseMoveEvent, value);
    }

    /// <summary>
    /// 点击预览事件（隧道）。
    /// </summary>
    public event RoutedEventHandler? PreviewClick
    {
        add => AddHandler(PreviewClickEvent, value);
        remove => RemoveHandler(PreviewClickEvent, value);
    }

    /// <summary>
    /// 获得焦点预览事件（隧道）。
    /// </summary>
    public event RoutedEventHandler? PreviewGotFocus
    {
        add => AddHandler(PreviewGotFocusEvent, value);
        remove => RemoveHandler(PreviewGotFocusEvent, value);
    }

    /// <summary>
    /// 失去焦点预览事件（隧道）。
    /// </summary>
    public event RoutedEventHandler? PreviewLostFocus
    {
        add => AddHandler(PreviewLostFocusEvent, value);
        remove => RemoveHandler(PreviewLostFocusEvent, value);
    }

    #endregion

    #region 命中测试

    /// <summary>
    /// 确定给定的点是否在此元素的渲染区域内。
    /// </summary>
    /// <param name="point">测试点</param>
    /// <returns>如果点在元素内则返回 true，否则返回 false</returns>
    public override bool HitTest(PointF point) =>
        IsEnabled &&
        Visibility == Visibility.Visible &&
        RenderBounds.Contains(point);

    /// <summary>
    /// 在指定点执行命中测试，返回最底层的命中元素。
    /// </summary>
    /// <param name="point">测试点</param>
    /// <returns>命中的元素，如果没有命中则返回 null</returns>
    public virtual UIElement? HitTestElement(PointF point) =>
        HitTest(point) ? this : null;

    #endregion

    #region 受保护方法 - 路由事件触发

    /// <summary>
    /// 触发 MouseDown 事件（隧道 + 冒泡）。
    /// </summary>
    protected virtual void OnMouseDown()
    {
        // 先触发隧道事件（从根到源）
        RaiseTunnelEvent(new RoutedEventArgs(PreviewMouseDownEvent, this));

        // 如果事件未被标记为已处理，则触发冒泡事件
        var args = new RoutedEventArgs(MouseDownEvent, this);
        RaiseEvent(args);
    }

    /// <summary>
    /// 触发 MouseUp 事件（隧道 + 冒泡）。
    /// </summary>
    protected virtual void OnMouseUp()
    {
        // 先触发隧道事件（从根到源）
        RaiseTunnelEvent(new RoutedEventArgs(PreviewMouseUpEvent, this));

        // 如果事件未被标记为已处理，则触发冒泡事件
        var args = new RoutedEventArgs(MouseUpEvent, this);
        RaiseEvent(args);
    }

    /// <summary>
    /// 触发 MouseMove 事件（隧道 + 冒泡）。
    /// </summary>
    protected virtual void OnMouseMove()
    {
        // 先触发隧道事件（从根到源）
        RaiseTunnelEvent(new RoutedEventArgs(PreviewMouseMoveEvent, this));

        // 如果事件未被标记为已处理，则触发冒泡事件
        var args = new RoutedEventArgs(MouseMoveEvent, this);
        RaiseEvent(args);
    }

    /// <summary>
    /// 触发 Click 事件（隧道 + 冒泡）。
    /// </summary>
    protected virtual void OnClick()
    {
        // 先触发隧道事件（从根到源）
        RaiseTunnelEvent(new RoutedEventArgs(PreviewClickEvent, this));

        // 如果事件未被标记为已处理，则触发冒泡事件
        var args = new RoutedEventArgs(ClickEvent, this);
        RaiseEvent(args);
    }

    /// <summary>
    /// 触发 GotFocus 事件（隧道 + 冒泡）。
    /// </summary>
    protected virtual void OnGotFocus()
    {
        // 先触发隧道事件（从根到源）
        RaiseTunnelEvent(new RoutedEventArgs(PreviewGotFocusEvent, this));

        // 如果事件未被标记为已处理，则触发冒泡事件
        var args = new RoutedEventArgs(GotFocusEvent, this);
        RaiseEvent(args);
    }

    /// <summary>
    /// 触发 LostFocus 事件（隧道 + 冒泡）。
    /// </summary>
    protected virtual void OnLostFocus()
    {
        // 先触发隧道事件（从根到源）
        RaiseTunnelEvent(new RoutedEventArgs(PreviewLostFocusEvent, this));

        // 如果事件未被标记为已处理，则触发冒泡事件
        var args = new RoutedEventArgs(LostFocusEvent, this);
        RaiseEvent(args);
    }

    #endregion

    #region 路由事件路由实现

    /// <summary>
    /// 触发隧道路由事件（从根元素到源元素）。
    /// </summary>
    /// <param name="e">路由事件参数</param>
    protected void RaiseTunnelEvent(RoutedEventArgs e)
    {
        if (e.RoutedEvent == null)
            throw new InvalidOperationException("路由事件参数必须设置 RoutedEvent 属性。");

        if (e.RoutedEvent.RoutingStrategy != RoutingStrategy.Tunnel)
            throw new InvalidOperationException("此方法仅用于触发隧道事件。");

        // 收集从根到源的元素路径
        var route = BuildRouteToSource();

        // 从根到源调用处理程序
        foreach (var element in route)
        {
            element.InvokeEventHandlersForRoute(e.RoutedEvent, e, false);

            // 如果事件被标记为已处理，停止路由
            if (e.Handled)
                break;
        }
    }

    /// <summary>
    /// 重写 RaiseEvent 以支持冒泡路由事件（从源元素到根元素）。
    /// </summary>
    public override void RaiseEvent(RoutedEventArgs e)
    {
        if (e.RoutedEvent == null)
            throw new InvalidOperationException("路由事件参数必须设置 RoutedEvent 属性。");

        if (e.RoutedEvent.RoutingStrategy != RoutingStrategy.Bubble)
            throw new InvalidOperationException("此方法仅用于触发冒泡事件。");

        // 收集从源到根的元素路径
        var route = BuildRouteToRoot();

        // 从源到根调用处理程序
        foreach (var element in route)
        {
            element.InvokeEventHandlersForRoute(e.RoutedEvent, e, false);

            // 如果事件被标记为已处理，停止路由
            if (e.Handled)
                break;
        }
    }

    /// <summary>
    /// 构建从当前元素到根元素的冒泡路由路径。
    /// </summary>
    /// <returns>元素路径（从当前元素开始）</returns>
    private List<UIElement> BuildRouteToRoot()
    {
        var route = new List<UIElement>();
        var current = this;

        while (current is not null)
        {
            route.Add(current);
            current = current.Parent as UIElement;
        }

        return route;
    }

    /// <summary>
    /// 构建从根元素到当前元素的隧道路由路径。
    /// </summary>
    /// <returns>元素路径（从根元素开始）</returns>
    private List<UIElement> BuildRouteToSource()
    {
        var route = BuildRouteToRoot();
        route.Reverse(); // 反转，使路径从根到源
        return route;
    }

    #endregion

    #region 焦点管理

    /// <summary>
    /// 请求焦点到此控件。
    /// </summary>
    /// <returns>如果成功获得焦点则返回 true，否则返回 false</returns>
    public virtual bool Focus()
    {
        if (!IsEnabled || Visibility != Visibility.Visible)
            return false;

        OnGotFocus();
        return true;
    }

    /// <summary>
    /// 释放焦点。
    /// </summary>
    public virtual void Unfocus()
    {
        OnLostFocus();
    }

    #endregion

    public event UIEventHandler<UIElement>? Initialize;
    public event UIEventHandler<UIElement, UpdateEventArgs>? Update;

    protected internal virtual void OnInitialize()
        => Initialize?.Invoke(this, EventArgs.Empty);

    protected internal virtual void OnUpdate(double deltaTime)
        => Update?.Invoke(this, new(deltaTime));
}