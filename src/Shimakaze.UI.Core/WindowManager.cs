using Microsoft.Extensions.DependencyInjection;

namespace Shimakaze.UI.Core;

public sealed class WindowManager
{
    private readonly LinkedList<INativeWindow> _windows = [];

    private bool _initialized = false;
    public bool IsEmpty => _windows.Count == 0;

    internal void Register(Window window)
    {
        if (_windows.Contains(window))
            return;

        INativeWindow nativeWindow = window;
        var node = _windows.AddLast(window);
        window.Closed += (_, _) => _windows.Remove(node);

        if (_initialized)
            nativeWindow.Native.Initialize();
    }

    internal void Initialize()
    {
        if (_initialized)
            throw new InvalidOperationException("WindowManager is already initialized.");

        _initialized = true;

        // 什么也不做 因为 Window 构造方法已经注册了
        _ = Application.Instance.Services.GetServices<Window>();

        var node = _windows.First;
        while (node != null)
        {
            var window = node.Value.Native;
            window.Initialize();
            node = node.Next;
        }
    }

    internal void Update()
    {
        var node = _windows.First;
        while (node != null)
        {
            var window = node.Value.Native;
            window.DoEvents();
            window.DoUpdate();
            window.DoRender();
            node = node.Next;
        }
    }
}