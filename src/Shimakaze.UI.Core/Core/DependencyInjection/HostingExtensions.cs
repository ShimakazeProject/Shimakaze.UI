using Shimakaze.UI;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.Hosting;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class HostingExtensions
{
    extension(Host)
    {
        /// <inheritdoc cref="ShimakazeUIApplicationBuilder()" />
        public static ShimakazeUIApplicationBuilder CreateShimakazeUIApplicationBuilder() => new();

        /// <inheritdoc cref="ShimakazeUIApplicationBuilder(string[]?)" />
        public static ShimakazeUIApplicationBuilder CreateShimakazeUIApplicationBuilder(string[]? args) => new(args);

        /// <inheritdoc cref="ShimakazeUIApplicationBuilder(ShimakazeUIApplicationBuilderSettings?)" />
        public static ShimakazeUIApplicationBuilder CreateShimakazeUIApplicationBuilder(ShimakazeUIApplicationBuilderSettings? settings) => new(settings);
    }
}