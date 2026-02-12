namespace Shimakaze.UI;

/// <summary>
/// 路由事件参数基类。
/// </summary>
public class RoutedEventArgs : EventArgs
{
    /// <summary>
    /// 初始化 RoutedEventArgs 类的新实例。
    /// </summary>
    public RoutedEventArgs()
    {
    }

    /// <summary>
    /// 初始化 RoutedEventArgs 类的新实例，使用指定的事件。
    /// </summary>
    /// <param name="routedEvent">要标识的路由事件</param>
    public RoutedEventArgs(RoutedEvent routedEvent)
    {
        RoutedEvent = routedEvent;
    }

    /// <summary>
    /// 初始化 RoutedEventArgs 类的新实例，使用指定的事件和源元素。
    /// </summary>
    /// <param name="routedEvent">要标识的路由事件</param>
    /// <param name="source">触发事件的元素</param>
    public RoutedEventArgs(RoutedEvent routedEvent, object? source)
    {
        RoutedEvent = routedEvent;
        Source = source;
    }

    /// <summary>
    /// 获取或设置一个值，该值指示是否处理了路由事件。
    /// </summary>
    public bool Handled { get; set; }

    /// <summary>
    /// 获取或设置路由事件的路由事件。
    /// </summary>
    public RoutedEvent? RoutedEvent { get; set; }

    /// <summary>
    /// 获取或设置报告了事件的源对象。
    /// </summary>
    public object? Source { get; set; }

    /// <summary>
    /// 获取或设置对与事件关联的对象的引用。
    /// </summary>
    /// <remarks>通常此值与 Source 相同，但在事件冒泡或隧道过程中可能会改变。</remarks>
    public object? OriginalSource { get; set; }
}
