using Shimakaze.UI.Glfw;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseGlfw(this IServiceCollection services)
        => services.UseGlfwWindowProvider();

    public static IServiceCollection UseGlfwWindowProvider(this IServiceCollection services)
        => services.UseWindowProvider<GlfwWindowProvider>();
}