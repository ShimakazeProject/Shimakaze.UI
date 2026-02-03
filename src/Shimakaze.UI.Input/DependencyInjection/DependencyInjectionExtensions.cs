using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Shimakaze.UI.Input;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseInputContextProvider
        <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TInputContextProvider>(
        this IServiceCollection services)
        where TInputContextProvider : class, IInputContextProvider
    {
        services.TryAddTransient<InputManager>();
        
        services.TryAddSingleton<TInputContextProvider>();
        services.TryAddSingleton<IInputContextProvider>(provider => provider.GetRequiredService<TInputContextProvider>());
        return services;
    }
}