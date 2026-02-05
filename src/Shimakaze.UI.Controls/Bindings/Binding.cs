
using System.ComponentModel;
using System.Reflection;

namespace Shimakaze.UI.Bindings;

public sealed class Binding
{
    private BindingMode _mode;
    private readonly WeakReference<object> _source;
    private readonly WeakReference<object> _target;
    private readonly PropertyInfo _sourceProperty;
    private readonly PropertyInfo _targetProperty;
    private volatile bool _isUpdating;

    public Binding(
        BindingMode mode,
        object target,
        object source,
        PropertyInfo sourcePropertyInfo,
        PropertyInfo targetPropertyInfo)
    {
        _mode = mode;
        _source = new(source);
        _target = new(target);
        _sourceProperty = sourcePropertyInfo;
        _targetProperty = targetPropertyInfo;

        SetOneWay(source);
        SetOneWayToSource(target);
        if (mode is not BindingMode.OneWayToSource)
            _targetProperty.SetValue(target, _sourceProperty.GetValue(source));
    }

    private void SetOneWay(object source)
    {
        if (!_mode.HasFlag(BindingMode.OneWay))
            return;

        if (source is not INotifyPropertyChanged notify)
        {
            _mode &= ~BindingMode.OneWay;
            return;
        }

        notify.PropertyChanged += OnSourcePropertyChanged;
    }

    private void SetOneWayToSource(object target)
    {
        if (!_mode.HasFlag(BindingMode.OneWayToSource))
            return;

        if (target is not INotifyPropertyChanged notify)
        {
            _mode &= ~BindingMode.OneWayToSource;
            return;
        }

        notify.PropertyChanged += OnTargetPropertyChanged;
    }

    private void OnSourcePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != _sourceProperty.Name)
            return;

        if (!_source.TryGetTarget(out var source) || source is null)
            return;

        if (!_target.TryGetTarget(out var target) || target is null)
            return;

        if (Interlocked.CompareExchange(ref _isUpdating, true, false))
            return;
        try
        {
            _targetProperty.SetValue(target, _sourceProperty.GetValue(source));
        }
        finally
        {
            Interlocked.Exchange(ref _isUpdating, false);
        }
    }

    private void OnTargetPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != _targetProperty.Name)
            return;

        if (!_source.TryGetTarget(out var source) || source is null)
            return;

        if (!_target.TryGetTarget(out var target) || target is null)
            return;

        if (Interlocked.CompareExchange(ref _isUpdating, true, false))
            return;
        try
        {
            _sourceProperty.SetValue(source, _targetProperty.GetValue(target));
        }
        finally
        {
            Interlocked.Exchange(ref _isUpdating, false);
        }
    }
}