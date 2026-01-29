using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Shimakaze.UI.Core.Dispatchers;

namespace Shimakaze.UI.Core;

/// <inheritdoc cref="HostApplicationBuilder" />
public sealed class ShimakazeUIApplicationBuilder : IHostApplicationBuilder
{
    private readonly IHostApplicationBuilder _builder;

    #region Constuctors
    /// <inheritdoc cref="HostApplicationBuilder()" />
    public ShimakazeUIApplicationBuilder()
        : this(new HostApplicationBuilder())
    { }

    /// <inheritdoc cref="HostApplicationBuilder(string[]?)" />
    public ShimakazeUIApplicationBuilder(string[]? args)
        : this(new HostApplicationBuilder(args))
    { }

    /// <inheritdoc cref="HostApplicationBuilder(HostApplicationBuilderSettings?)" />
    public ShimakazeUIApplicationBuilder(ShimakazeUIApplicationBuilderSettings? settings)
        : this(new HostApplicationBuilder(settings?.Settings))
    { }

    internal ShimakazeUIApplicationBuilder(IHostApplicationBuilder builder)
        => _builder = builder;
    #endregion

    #region IHostApplicationBuilder
    public IDictionary<object, object> Properties => _builder.Properties;

    public IConfigurationManager Configuration => _builder.Configuration;

    public IHostEnvironment Environment => _builder.Environment;

    public ILoggingBuilder Logging => _builder.Logging;

    public IMetricsBuilder Metrics => _builder.Metrics;

    public IServiceCollection Services => _builder.Services;

    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null)
        where TContainerBuilder : notnull
        => _builder.ConfigureContainer(factory, configure);
    #endregion

    public Application Build()
    {
        Services.TryAddSingleton<Dispatcher>();

        Services.TryAddSingleton(provider =>
        {
            return ActivatorUtilities.CreateInstance<Application>(provider);
        });

        var serviceProvider = Services.BuildServiceProvider();

        return serviceProvider.GetRequiredService<Application>();
    }
}