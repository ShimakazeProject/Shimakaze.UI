using Shimakaze.UI.Core;

namespace Microsoft.Extensions.Hosting;

public static class ShimakazeUIApplicationBuilderExtensions
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