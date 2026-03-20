using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Shimakaze.UI;

public sealed class ShimakazeUIApplicationBuilderSettings
{
    internal readonly HostApplicationBuilderSettings Settings = new();

    /// <inheritdoc cref="HostApplicationBuilderSettings.DisableDefaults" />
    public bool DisableDefaults
    {
        get => Settings.DisableDefaults;
        set => Settings.DisableDefaults = value;
    }

    /// <inheritdoc cref="HostApplicationBuilderSettings.Args" />
    public string[]? Args
    {
        get => Settings.Args;
        set => Settings.Args = value;
    }
    /// <inheritdoc cref="HostApplicationBuilderSettings.Configuration" />
    public ConfigurationManager? Configuration
    {
        get => Settings.Configuration;
        set => Settings.Configuration = value;
    }

    /// <inheritdoc cref="HostApplicationBuilderSettings.EnvironmentName" />
    public string? EnvironmentName
    {
        get => Settings.EnvironmentName;
        set => Settings.EnvironmentName = value;
    }

    /// <inheritdoc cref="HostApplicationBuilderSettings.ApplicationName" />
    public string? ApplicationName
    {
        get => Settings.ApplicationName;
        set => Settings.ApplicationName = value;
    }

    /// <inheritdoc cref="HostApplicationBuilderSettings.ContentRootPath" />
    public string? ContentRootPath
    {
        get => Settings.ContentRootPath;
        set => Settings.ContentRootPath = value;
    }
}