using Shimakaze.UI.Data;

namespace Shimakaze.UI;


public abstract class DependencyObject
{
    // 存储每个已设置值的属性（仅非默认值）
    private readonly Dictionary<int, object?> _localValues = [];

    // 绑定表达式存储（每个属性最多一个绑定）
    private readonly Dictionary<int, BindingExpression> _bindings = [];

    // 可选：值来源标记（用于 GetValue 优先级决策）
    private readonly Dictionary<int, ValueSource> _valueSources = [];


    // 公开的 SetValue（设置本地值，最高优先级）
    public void SetValue(DependencyProperty dp, object? value)
    {
        // 类型检查、强制回调...

        // 本地值总是覆盖绑定和默认值
        _localValues[dp.GlobalIndex] = value;
        _valueSources[dp.GlobalIndex] = ValueSource.Local;

        // 如果存在绑定，需要断开（本地值 > 绑定）
        if (_bindings.Remove(dp.GlobalIndex, out var expr))
            expr.Detach();

        // 触发回调
        dp.DefaultMetadata.PropertyChangedCallback?.Invoke(this,
            new DependencyPropertyChangedEventArgs(dp, null, value));

        // 双向绑定：如果该属性原本有绑定，且模式为 TwoWay 或 OneWayToSource，
        // 应将本地值写回源（模拟 UpdateSourceTrigger）
        // 这里简化：只要设置了本地值，就立即尝试更新源（类似 PropertyChanged）
        expr?.UpdateSource(value);
    }

    // 获取值（根据优先级）
    public object? GetValue(DependencyProperty dp)
    {
        var idx = dp.GlobalIndex;

        // 1. 本地值
        if (_localValues.TryGetValue(idx, out var local))
            return local;

        // 2. 绑定值（如果来源标记为 Binding，且绑定表达式还存在）
        if (_valueSources.TryGetValue(idx, out var src) && src == ValueSource.Binding &&
            _bindings.TryGetValue(idx, out var expr))
        {
            // 注意：BindingExpression 应缓存值或实时计算
            // 为简化，这里直接调用 UpdateTarget 并读取目标值？不，应直接取表达式内部缓存。
            // 建议 BindingExpression 增加一个 GetRawValue() 方法，返回上次计算的值。
            return expr.GetCurrentValue(); // 需在 BindingExpression 中实现缓存
        }

        // 3. 默认值
        return dp.DefaultMetadata.DefaultValue;
    }


    // 公开的 SetBinding 入口
    public void SetBinding(DependencyProperty dp, Binding binding)
    {
        // 如果该属性已有绑定，先断开旧的
        if (_bindings.Remove(dp.GlobalIndex, out var oldExpr))
            oldExpr.Detach();

        // 创建新绑定表达式
        var expr = new BindingExpression(binding, this, dp);
        _bindings[dp.GlobalIndex] = expr;

        // 将值来源标记为 Binding（但不覆盖已存在的本地值）
        _valueSources[dp.GlobalIndex] = ValueSource.Binding;
    }

    // 内部设置值的方法，可指定来源（用于绑定更新）
    internal void SetValueInternal(DependencyProperty dp, object? value, ValueSource source)
    {
        // 类型检查、强制回调（与 SetValue 共用逻辑）
        // ...

        // 根据来源决定是否覆盖现有值
        var currentSource = _valueSources.GetValueOrDefault(dp.GlobalIndex, ValueSource.Default);
        if (source >= currentSource) // 优先级：Local > Binding > Default
        {
            _localValues[dp.GlobalIndex] = value;
            _valueSources[dp.GlobalIndex] = source;

            // 触发属性变更回调
            dp.DefaultMetadata.PropertyChangedCallback?.Invoke(this,
                new DependencyPropertyChangedEventArgs(dp, null, value));
        }
    }
}