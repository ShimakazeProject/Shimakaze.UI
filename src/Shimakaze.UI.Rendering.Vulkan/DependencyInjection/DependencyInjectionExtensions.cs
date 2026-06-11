using Shimakaze.UI.Rendering.Vulkan;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseVulkan(this IServiceCollection services) => services
        .UseVulkanWindowOptionsProvider()
        .UseRendererProvider(i => i.GetRequiredService<VulkanProvider>());

    public static IServiceCollection UseVulkanRenderer(this IServiceCollection services)
        => services.UseRendererProvider<VulkanProvider>();

    public static IServiceCollection UseVulkanWindowOptionsProvider(this IServiceCollection services)
        => services.UseWindowOptionsProvider<VulkanProvider>();
}