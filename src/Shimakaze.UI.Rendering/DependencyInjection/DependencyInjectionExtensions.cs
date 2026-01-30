using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Shimakaze.UI.Rendering;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseRenderer
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRenderer>(
        this IServiceCollection services)
        where TRenderer : class, IRenderer
    {
        services.TryAddSingleton<TRenderer>();
        services.TryAddSingleton<IRenderer>(provider => provider.GetRequiredService<TRenderer>());
        return services;
    }
}