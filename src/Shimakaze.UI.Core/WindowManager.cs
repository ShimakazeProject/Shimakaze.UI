using Microsoft.Extensions.DependencyInjection;

namespace Shimakaze.UI.Core;

public sealed class WindowManager
{
    private readonly LinkedList<PlatformWindow> _windows = [];
    private readonly Lock _lock = new();

    private bool _initialized = false;
    public bool IsEmpty => _windows.Count == 0;

    internal void Register(PlatformWindow window)
    {
        if (_windows.Contains(window))
            return;

        var node = _windows.AddLast(window);
        window.Closed += (_, _) => _windows.Remove(node);

        lock (_lock)
        {
            if (_initialized)
                window.Native.Initialize();
        }
    }

    internal void Initialize()
    {
        lock (_lock)
        {
            if (_initialized)
                throw new InvalidOperationException("WindowManager is already initialized.");

            // 什么也不做 因为 Window 构造方法已经注册了
            _ = Application.Instance.Services.GetServices<PlatformWindow>();
            _ = Application.Instance.Services.GetServices<IPlatformWindowWrap>();

            var node = _windows.First;
            while (node != null)
            {
                var window = node.Value.Native;
                window.Initialize();
                node = node.Next;
            }

            _initialized = true;
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