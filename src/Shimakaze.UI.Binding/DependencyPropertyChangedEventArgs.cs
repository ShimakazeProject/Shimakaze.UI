namespace Shimakaze.UI;

public readonly record struct DependencyPropertyChangedEventArgs(DependencyProperty Property, object? OldValue, object? NewValue);