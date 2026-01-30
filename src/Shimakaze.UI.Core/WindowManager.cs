
using Microsoft.Extensions.DependencyInjection;

using Silk.NET.Input;
using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public sealed class WindowManager(
    IServiceProvider serviceProvider,
    IWindowOptionsProvider windowOptionsProvider,
    IWindowProvider windowProvider,
    IInputContextProvider inputContextProvider)
{
    public TWindow CreateWindow<TWindow>()
        where TWindow : Window
    {
        var window = ActivatorUtilities.CreateInstance<TWindow>(serviceProvider);

        return window;
    }

    internal IInputContext CreateInputContext(IWindow native)
        => inputContextProvider.CreateInputContext(native);

    internal IWindow CreateNativeWindow()
        => windowProvider.CreateWindow(
            windowOptionsProvider.CreateOptions());
}