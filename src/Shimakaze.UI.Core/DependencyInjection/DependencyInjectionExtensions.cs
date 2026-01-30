using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Shimakaze.UI.Core;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseWindowProvider
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWindowProvider>(
        this IServiceCollection services)
        where TWindowProvider : class, IWindowProvider
    {
        services.TryAddSingleton<TWindowProvider>();
        services.TryAddSingleton<IWindowProvider>(provider => provider.GetRequiredService<TWindowProvider>());
        return services;
    }

    public static IServiceCollection UseWindowOptionsProvider
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWindowOptionsProvider>(
        this IServiceCollection services)
        where TWindowOptionsProvider : class, IWindowOptionsProvider
    {
        services.TryAddSingleton<TWindowOptionsProvider>();
        services.TryAddSingleton<IWindowOptionsProvider>(provider => provider.GetRequiredService<TWindowOptionsProvider>());
        return services;
    }
}