using System.Reflection;

namespace Shimakaze.UI.Data;

public class Binding(BindingMode mode, object source, PropertyInfo property)
{
    public object Source { get; } = source;
    public PropertyInfo Property { get; } = property;
    public BindingMode Mode { get; } = mode;
    public IValueConverter? Converter { get; set; }
}
