namespace Shimakaze.UI;

public class PropertyMetadata(object? defaultValue = null, PropertyChangedCallback? propertyChangedCallback = null, CoerceValueCallback? coerceValueCallback = null)
{
    public CoerceValueCallback? CoerceValueCallback { get; set; } = coerceValueCallback;
    public object? DefaultValue { get; set; } = defaultValue;
    protected bool IsSealed { get; } = coerceValueCallback != null;
    public PropertyChangedCallback? PropertyChangedCallback { get; set; } = propertyChangedCallback;

    public PropertyMetadata(PropertyChangedCallback propertyChangedCallback)
        : this(null, propertyChangedCallback)
    { }
}