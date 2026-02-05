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
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRendererProvider>(
        this IServiceCollection services)
        where TRendererProvider : class, IRendererProvider
    {
        services.TryAddSingleton<TRendererProvider>();
        if (typeof(TRendererProvider).IsAssignableTo(typeof(RendererProvider)))
        {
            services.TryAddSingleton<RendererProvider>(provider =>
            {
                var result = provider.GetRequiredService<TRendererProvider>() as RendererProvider;
                Debug.Assert(result is not null);
                return result;
            });
        }

        services.TryAddSingleton<IRendererProvider>(provider => provider.GetRequiredService<TRendererProvider>());
        return services;
    }
}