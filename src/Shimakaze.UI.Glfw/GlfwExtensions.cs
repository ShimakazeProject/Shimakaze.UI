using Shimakaze.UI.Core;
using Shimakaze.UI.Glfw;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class GlfwExtensions
{
    extension(ShimakazeUIApplicationBuilder builder)
    {
        public ShimakazeUIApplicationBuilder UseGlfw()
        {
            builder.Services.UseWindowProvider<GlfwWindowProvider>();

            return builder;
        }
    }
}