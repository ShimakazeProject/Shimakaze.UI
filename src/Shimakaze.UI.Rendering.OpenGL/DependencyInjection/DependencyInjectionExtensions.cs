using Shimakaze.UI.Rendering.OpenGL;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseOpenGL(this IServiceCollection services) => services
        .UseOpenGLWindowOptionsProvider()
        .UseRendererProvider(i => i.GetRequiredService<OpenGLProvider>());

    public static IServiceCollection UseOpenGLRenderer(this IServiceCollection services)
        => services.UseRendererProvider<OpenGLProvider>();

    public static IServiceCollection UseOpenGLWindowOptionsProvider(this IServiceCollection services)
        => services.UseWindowOptionsProvider<OpenGLProvider>();
}