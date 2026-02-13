namespace Shimakaze.UI;

/// <summary>
/// 提供路由事件注册和管理的方法。
/// </summary>
public static class EventManager
{
    /// <summary>
    /// 注册一个新的路由事件。
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <param name="routingStrategy">路由策略</param>
    /// <param name="handlerType">事件处理程序的类型</param>
    /// <param name="ownerType">拥有此事件的类型</param>
    /// <returns>已注册的路由事件</returns>
    public static RoutedEvent RegisterRoutedEvent(
        string name,
        RoutingStrategy routingStrategy,
        Type handlerType,
        Type ownerType)
    {
        return RoutedEvent.Register(name, routingStrategy, handlerType, ownerType);
    }

    /// <summary>
    /// 注册一个类处理程序，以便为指定路由事件调用。
    /// </summary>
    /// <param name="classType">要注册类处理程序的类类型</param>
    /// <param name="routedEvent">要处理的路由事件</param>
    /// <param name="handler">要注册的处理程序</param>
    /// <param name="handledEventsToo">如果为 true，即使事件被标记为已处理也调用处理程序</param>
    public static void RegisterClassHandler(
        Type classType,
        RoutedEvent routedEvent,
        Delegate handler,
        bool handledEventsToo = false)
    {
        _ = classType;
        _ = routedEvent;
        _ = handler;
        _ = handledEventsToo;

        // 注意：需要在 DependencyObject 或 UIElement 中维护类处理程序存储
        // 这是一个简化实现，实际应用中需要更复杂的类处理程序管理机制
        throw new NotImplementedException("类处理程序功能尚未实现。");
    }
}