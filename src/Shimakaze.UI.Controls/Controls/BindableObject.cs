using System.Reflection;

using Shimakaze.UI.Bindings;

namespace Shimakaze.UI.Controls;

public abstract class BindableObject : ObservableObject
{
    private readonly Dictionary<PropertyInfo, Binding> _bindings = [];

    /// <summary>
    /// 添加数据绑定。
    /// </summary>
    /// <param name="binding">要添加的绑定</param>
    internal void AddBinding(PropertyInfo propertyInfo, Binding binding)
        => _bindings[propertyInfo] = binding;

    /// <summary>
    /// 清除所有绑定。
    /// </summary>
    public void ClearBindings()
        => _bindings.Clear();
}