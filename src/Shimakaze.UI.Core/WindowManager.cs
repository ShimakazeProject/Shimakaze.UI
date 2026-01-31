using Microsoft.Extensions.DependencyInjection;

namespace Shimakaze.UI.Core;

public sealed class WindowManager
{
    private readonly LinkedList<INativeWindow> _windows = [];
    private readonly Lock _lock = new();

    private bool _initialized = false;
    public bool IsEmpty => _windows.Count == 0;

    internal void Register(Window window)
    {
        if (_windows.Contains(window))
            return;

        INativeWindow nativeWindow = window;
        LinkedListNode<INativeWindow> node;
        lock (_lock)
        {
            node = _windows.AddLast(window);
        }

        window.Closed += (_, _) =>
        {
            lock (_lock)
            {
                _windows.Remove(node);
            }
        };

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

        lock (_lock)
        {
            foreach (var window in _windows)
                window.Native.Initialize();
        }
    }

    internal void Update()
    {
        lock (_lock)
        {
            foreach (var window in _windows)
            {
                window.Native.DoEvents();
                window.Native.DoUpdate();
                window.Native.DoRender();
            }
        }
    }
}