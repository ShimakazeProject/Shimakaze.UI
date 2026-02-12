using System.Collections.Concurrent;

namespace Shimakaze.UI;

public sealed class DependencyProperty
{
    private static int s_globalIndexCounter;
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, Dictionary<string, DependencyProperty>> Registered = [];

    public string Name { get; }
    public Type PropertyType { get; }
    public Type OwnerType { get; }
    public PropertyMetadata DefaultMetadata { get; }
    public int GlobalIndex { get; }

    private DependencyProperty(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata)
    {
        Name = name;
        PropertyType = propertyType;
        OwnerType = ownerType;
        DefaultMetadata = defaultMetadata;
        GlobalIndex = Interlocked.Increment(ref s_globalIndexCounter);
    }

    public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata? defaultMetadata = null)
    {
        var handle = ownerType.TypeHandle;
        var props = Registered.GetOrAdd(handle, _ => []);

        if (props.TryGetValue(name, out var existing))
            throw new InvalidOperationException($"属性 {name} 已经在 {ownerType} 中注册。");

        DependencyProperty dp = new(name, propertyType, ownerType, defaultMetadata ?? new());
        props[name] = dp;
        return dp;
    }
}
