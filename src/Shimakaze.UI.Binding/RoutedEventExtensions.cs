namespace Shimakaze.UI;

/// <summary>
/// 路由事件相关的扩展方法。
/// </summary>
public static class RoutedEventExtensions
{
    /// <summary>
    /// 为指定对象添加路由事件处理程序。
    /// </summary>
    /// <param name="obj">要添加处理程序的对象</param>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="handler">事件处理程序</param>
    public static void AddHandler(this DependencyObject obj, RoutedEvent routedEvent, RoutedEventHandler handler)
    {
        obj.AddHandler(routedEvent, handler, false);
    }

    /// <summary>
    /// 为指定对象添加路由事件处理程序，即使事件被处理也调用。
    /// </summary>
    /// <param name="obj">要添加处理程序的对象</param>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="handler">事件处理程序</param>
    /// <param name="handledEventsToo">是否处理已标记为已处理的事件</param>
    public static void AddHandler(this DependencyObject obj, RoutedEvent routedEvent, RoutedEventHandler handler, bool handledEventsToo)
    {
        obj.AddHandler(routedEvent, handler, handledEventsToo);
    }

    /// <summary>
    /// 从指定对象移除路由事件处理程序。
    /// </summary>
    /// <param name="obj">要移除处理程序的对象</param>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="handler">事件处理程序</param>
    public static void RemoveHandler(this DependencyObject obj, RoutedEvent routedEvent, RoutedEventHandler handler)
    {
        obj.RemoveHandler(routedEvent, handler);
    }
}