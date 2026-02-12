using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Shimakaze.UI.Data;

public class BindingExpression
{
    private readonly Binding _binding;
    private readonly DependencyObject _target;
    private readonly DependencyProperty _dp;
    private readonly object _source;
    private readonly PropertyInfo _sourceProperty;
    private readonly INotifyPropertyChanged? _notifySource;
    private object? _cachedValue;

    public BindingExpression(Binding binding, DependencyObject target, DependencyProperty dp)
    {
        _binding = binding;
        _target = target;
        _dp = dp;
        _source = binding.Source;
        _sourceProperty = binding.Property;

        // 订阅源属性变更（仅当需要监听源→目标）
        if (binding.Mode.HasFlag(BindingMode.OneWay))
        {
            _notifySource = _source as INotifyPropertyChanged;
            if (_notifySource != null)
                _notifySource.PropertyChanged += OnSourcePropertyChanged;
        }

        // 初始化推送一次源值到目标
        UpdateTarget();
    }

    private void OnSourcePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == _sourceProperty.Name)
            UpdateTarget();
    }

    // 从源读取值，应用转换器，写入目标
    public void UpdateTarget()
    {
        var raw = _sourceProperty.GetValue(_source);
        _cachedValue = _binding.Converter?.Convert(raw, _dp.PropertyType, null, CultureInfo.CurrentCulture)
                       ?? raw;
        _target.SetValueInternal(_dp, _cachedValue, ValueSource.Binding);
    }

    // 从目标读取值，应用反向转换器，写入源（双向模式时调用）
    public void UpdateSource(object? newValue)
    {
        if ((_binding.Mode & BindingMode.OneWayToSource) == 0)
            return;

        var converted = _binding.Converter?.ConvertBack(newValue, _sourceProperty.PropertyType, null, CultureInfo.CurrentCulture)
                      ?? newValue;
        _sourceProperty.SetValue(_source, converted);
    }

    // 解除订阅，避免内存泄漏
    public void Detach()
    {
        if (_notifySource != null)
            _notifySource.PropertyChanged -= OnSourcePropertyChanged;
    }

    public object? GetCurrentValue() => _cachedValue;
}