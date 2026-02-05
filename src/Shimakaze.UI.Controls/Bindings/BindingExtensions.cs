using System.Linq.Expressions;
using System.Reflection;

namespace Shimakaze.UI.Bindings;

public static class BindingExtensions
{
    public static Binding Bind<TTarget, TSource, TValue>(
        this TTarget target,
        BindingMode mode,
        TSource source,
        Expression<Func<TSource, TValue>> sourceProperty,
        Expression<Func<TTarget, TValue>> targetProperty)
        where TTarget : notnull
        where TSource : notnull
    {
        var sourceProp = GetPropertyInfo(sourceProperty);
        var targetProp = GetPropertyInfo(targetProperty);

        return new Binding(mode, target, source, sourceProp, targetProp);
    }

    private static PropertyInfo GetPropertyInfo<T, TValue>(Expression<Func<T, TValue>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        // 1. 剥离显式类型转换（支持 x => (object)x.Name）
        var body = expression.Body switch
        {
            UnaryExpression { NodeType: ExpressionType.Convert } unary => unary.Operand,
            var b => b
        };

        // 2. 匹配：必须是【直接作用于 Lambda 参数】的属性访问
        if (body is MemberExpression
            {
                Expression: ParameterExpression paramExpr,
                Member: PropertyInfo propInfo
            }
            && paramExpr == expression.Parameters[0]) // 引用相等（关键！）
            return propInfo;

        // 3. 诊断：根据表达式类型提供精准错误提示
        return body switch
        {
            MemberExpression => throw new ArgumentException(
                $"不支持嵌套属性（如 'x => x.Child.Property'）。仅支持直接属性：'x => x.Property'。检测到：{expression}",
                nameof(expression)),

            MethodCallExpression => throw new ArgumentException(
                $"不支持方法调用（如 'x => x.GetName()'）。请使用属性访问：'x => x.Property'。表达式：{expression}",
                nameof(expression)),

            _ => throw new ArgumentException(
                $"无效的属性表达式。必须为简单属性访问（可带类型转换），例如：'x => x.Property' 或 'x => (object)x.Property'。实际：{expression}",
                nameof(expression))
        };
    }
}