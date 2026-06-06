using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Shimakaze.UI.Rendering;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseRendererProvider
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TSurfaceProviderFactory>(this IServiceCollection services)
        where TSurfaceProviderFactory : class, ISurfaceProviderFactory
    {
        services.TryAddSingleton<RendererProvider>();

        services.TryAddSingleton<TSurfaceProviderFactory>();

        services.TryAddSingleton<ISurfaceProviderFactory>(provider => provider.GetRequiredService<TSurfaceProviderFactory>());

        return services;
    }

    public static IServiceCollection UseRendererProvider
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TSurfaceProviderFactory>(
        this IServiceCollection services,
        Func<IServiceProvider, TSurfaceProviderFactory> factory)
        where TSurfaceProviderFactory : class, ISurfaceProviderFactory
    {
        services.TryAddSingleton<RendererProvider>();

        services.TryAddSingleton(factory);

        services.TryAddSingleton<ISurfaceProviderFactory>(provider => provider.GetRequiredService<TSurfaceProviderFactory>());

        return services;
    }
}