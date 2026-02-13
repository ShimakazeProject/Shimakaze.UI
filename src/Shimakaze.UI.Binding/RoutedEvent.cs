using System.Collections.Concurrent;

namespace Shimakaze.UI;

/// <summary>
/// 路由事件标识符，用于注册和标识路由事件。
/// </summary>
public sealed class RoutedEvent
{
    private static int s_globalIndexCounter;
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, Dictionary<string, RoutedEvent>> Registered = [];

    /// <summary>
    /// 获取路由事件的名称。
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 获取路由事件的所有者类型。
    /// </summary>
    public Type OwnerType { get; }

    /// <summary>
    /// 获取路由事件的参数类型。
    /// </summary>
    public Type HandlerType { get; }

    /// <summary>
    /// 获取路由事件的策略。
    /// </summary>
    public RoutingStrategy RoutingStrategy { get; }

    /// <summary>
    /// 获取路由事件的全局索引。
    /// </summary>
    public int GlobalIndex { get; }

    private RoutedEvent(string name, Type ownerType, Type handlerType, RoutingStrategy routingStrategy)
    {
        Name = name;
        OwnerType = ownerType;
        HandlerType = handlerType;
        RoutingStrategy = routingStrategy;
        GlobalIndex = Interlocked.Increment(ref s_globalIndexCounter);
    }

    /// <summary>
    /// 注册一个新的路由事件。
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <param name="routingStrategy">路由策略</param>
    /// <param name="handlerType">事件处理程序的类型</param>
    /// <param name="ownerType">拥有此事件的类型</param>
    /// <returns>已注册的路由事件</returns>
    public static RoutedEvent Register(string name, RoutingStrategy routingStrategy, Type handlerType, Type ownerType)
    {
        var handle = ownerType.TypeHandle;
        var events = Registered.GetOrAdd(handle, _ => []);

        if (events.TryGetValue(name, out var existing))
            throw new InvalidOperationException($"路由事件 {name} 已经在 {ownerType} 中注册。");

        RoutedEvent re = new(name, ownerType, handlerType, routingStrategy);
        events[name] = re;
        return re;
    }
}