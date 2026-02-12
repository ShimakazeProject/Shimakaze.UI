using System.Collections.Concurrent;

using Microsoft.Extensions.DependencyInjection;

using Shimakaze.UI.Input;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Shimakaze.UI.Core;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class CoreExtensions
{
    private static readonly ConcurrentDictionary<PlatformWindow, InputManager> InputManagers = [];

    internal static InputManager GetInputManager(IInputContextProvider provider, PlatformWindow window)
    {
        var result = InputManagers.GetOrAdd(window, _ => new(provider, window));
        window.Closed += (_, _) => InputManagers.Remove(window, out _);

        return result;
    }


    extension(Application application)
    {
        public IInputContextProvider InputContextProvider => application.Services.GetRequiredService<IInputContextProvider>();
    }

    extension(PlatformWindow window)
    {
        public InputManager Input => GetInputManager(Application.Instance.InputContextProvider, window);
    }
}