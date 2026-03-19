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
        where TWindowProvider : class, PlatformWindowProvider
    {
        services.TryAddSingleton<TWindowProvider>();
        services.TryAddSingleton<PlatformWindowProvider>(provider => provider.GetRequiredService<TWindowProvider>());
        return services;
    }

    public static IServiceCollection UseWindowOptionsProvider
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWindowOptionsProvider>(
        this IServiceCollection services)
        where TWindowOptionsProvider : class, PlatformWindowOptionsProvider
    {
        services.TryAddSingleton<TWindowOptionsProvider>();
        services.TryAddSingleton<PlatformWindowOptionsProvider>(provider => provider.GetRequiredService<TWindowOptionsProvider>());
        return services;
    }

    public static IServiceCollection AddPlatformWindow<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWindow>(
        this IServiceCollection services)
        where TWindow : PlatformWindow
    {
        var descriptor = ServiceDescriptor.Transient<PlatformWindow, TWindow>();

        services.TryAddEnumerable(descriptor);

        return services;
    }

    public static IServiceCollection AddWindow<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWindow>(
        this IServiceCollection services)
        where TWindow : class, IPlatformWindowWrap
    {
        var descriptor = ServiceDescriptor.Transient<IPlatformWindowWrap, TWindow>();

        services.TryAddEnumerable(descriptor);

        return services;
    }
}