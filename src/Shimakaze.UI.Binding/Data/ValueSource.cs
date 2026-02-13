namespace Shimakaze.UI.Data;

internal enum ValueSource
{
    Default,    // 元数据默认值
    Binding,    // 绑定表达式提供的值
    Local       // 通过 SetValue 设置的本地值
}