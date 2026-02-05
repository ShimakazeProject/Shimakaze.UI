using System.Collections.Concurrent;

using Microsoft.Extensions.DependencyInjection;

using Shimakaze.UI.Input;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Shimakaze.UI.Core;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class CoreExtensions
{
    private static readonly ConcurrentDictionary<Window, InputManager> InputManagers = [];

    internal static InputManager Get(IInputContextProvider provider, Window window)
    {
        var result = InputManagers.GetOrAdd(window, _ => new(provider, window));
        window.Closed += (_, _) => InputManagers.Remove(window, out _);

        return result;
    }


    extension(Application application)
    {
        public IInputContextProvider InputContextProvider => application.Services.GetRequiredService<IInputContextProvider>();
    }

    extension(Window window)
    {
        public InputManager Input => Get(Application.Instance.InputContextProvider, window);
    }

}