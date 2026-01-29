
using Microsoft.Extensions.DependencyInjection;


namespace Shimakaze.UI.Core;

public sealed class WindowManager(
    IServiceProvider serviceProvider,
    IWindowOptionsProvider windowOptionsProvider,
    IWindowProvider windowProvider)
{
    public TWindow CreateWindow<TWindow>()
        where TWindow : Window
    {
        var window = ActivatorUtilities.CreateInstance<TWindow>(serviceProvider);

        ApplyWindow(window);

        return window;
    }

    internal void ApplyWindow(Window window)
    {
        var options = windowOptionsProvider.CreateOptions();

        var native = windowProvider.CreateWindow(options);

        window.Native = native;
    }
}