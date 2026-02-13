namespace Shimakaze.UI;

/// <summary>
/// 路由事件处理程序信息，包含处理程序委托和是否处理已处理事件的标志。
/// </summary>
/// <remarks>
/// 初始化 RoutedEventHandlerInfo 类的新实例。
/// </remarks>
/// <param name="handler">路由事件处理程序</param>
/// <param name="handledEventsToo">是否处理已标记为已处理的事件</param>
internal sealed class RoutedEventHandlerInfo(RoutedEventHandler handler, bool handledEventsToo)
{
    /// <summary>
    /// 获取路由事件处理程序。
    /// </summary>
    public RoutedEventHandler Handler { get; } = handler;

    /// <summary>
    /// 获取一个值，该值指示处理程序是否应在事件被标记为已处理时仍被调用。
    /// </summary>
    public bool HandledEventsToo { get; } = handledEventsToo;
}