using Microsoft.Extensions.DependencyInjection;

using Shimakaze.UI.Rendering;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Shimakaze.UI.Core;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class ApplicationExtensions
{
    extension(Application application)
    {
        public IRendererProvider RendererProvider
            => application.Services.GetRequiredService<IRendererProvider>();

        public static BaseRenderer GetRenderer(PlatformWindow window)
            => Application.Instance.RendererProvider.GetRenderer(window);
    }
}