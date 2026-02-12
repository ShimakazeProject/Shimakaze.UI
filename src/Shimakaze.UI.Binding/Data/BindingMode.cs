namespace Shimakaze.UI.Data;

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
    /// VM -> V
    /// </remarks>
    OneWay,
    /// <summary>
    /// 只监听目标对象的变化，不监听源对象的变化
    /// </summary>
    /// <remarks>
    /// V -> VM
    /// </remarks>
    OneWayToSource,
    /// <summary>
    /// 双向绑定
    /// </summary>
    /// <remarks>
    /// VM <-> V
    /// </remarks>
    TwoWay = OneWay | OneWayToSource,
}
