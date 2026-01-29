using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Shimakaze.UI.Core;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection UseWindowProvider
            <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWindowProvider>()
            where TWindowProvider : class, IWindowProvider
        {
            services.TryAddSingleton<TWindowProvider>();
            services.TryAddSingleton<IWindowProvider>(provider => provider.GetRequiredService<TWindowProvider>());
            return services;
        }

        public IServiceCollection UseWindowOptionsProvider
            <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TWindowOptionsProvider>()
            where TWindowOptionsProvider : class, IWindowOptionsProvider
        {
            services.TryAddSingleton<TWindowOptionsProvider>();
            services.TryAddSingleton<IWindowOptionsProvider>(provider => provider.GetRequiredService<TWindowOptionsProvider>());
            return services;
        }
    }
}