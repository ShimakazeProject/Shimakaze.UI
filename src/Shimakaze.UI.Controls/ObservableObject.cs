

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Shimakaze.UI;

public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(e);
        PropertyChanged?.Invoke(this, e);
    }
    protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(e);
        PropertyChanging?.Invoke(this, e);
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
    protected void OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
    }

    protected bool SetProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, newValue))
            return false;

        OnPropertyChanging(propertyName);

        field = newValue;

        OnPropertyChanged(propertyName);

        return true;
    }
}