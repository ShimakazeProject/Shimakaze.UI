namespace Shimakaze.UI;

/// <summary>
/// 路由事件的路由策略。
/// </summary>
public enum RoutingStrategy
{
    /// <summary>
    /// 冒泡路由：事件从源元素向上传递到可视化树的根元素。
    /// </summary>
    Bubble,

    /// <summary>
    /// 隧道路由：事件从根元素向下传递到源元素。
    /// </summary>
    Tunnel,

    /// <summary>
    /// 直接路由：事件仅在源元素上触发，不路由。
    /// </summary>
    Direct
}
