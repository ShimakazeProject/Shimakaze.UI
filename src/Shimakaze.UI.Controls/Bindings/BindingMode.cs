namespace Shimakaze.UI.Controls;

[Flags]
public enum BindingMode
{
    /// <summary>
    /// 只读取一次，不监听源对象的变化
    /// </summary>
    OneTime,
    /// <summary>
    /// 只监听源对象的变化，不监听目标对象的变化
    /// </summary>
    /// <remarks>
    /// Source -> Target
    /// </remarks>
    OneWay,
    /// <summary>
    /// 只监听目标对象的变化，不监听源对象的变化
    /// </summary>
    /// <remarks>
    /// Target -> Source
    /// </remarks>
    OneWayToSource,
    /// <summary>
    /// 双向绑定
    /// </summary>
    /// <remarks>
    /// Source <-> Target
    /// </remarks>
    TwoWay = OneWay | OneWayToSource,
}