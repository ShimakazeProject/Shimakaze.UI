namespace Shimakaze.UI.Core;

public sealed class WindowManager
{
    private readonly LinkedList<INativeWindow> _windows = [];

    private bool _initialized = false;

    internal void Register(Window window)
    {
        INativeWindow nativeWindow = window;
        var node = _windows.AddLast(window);
        window.Closing += (_, _) => _windows.Remove(node);

        if (_initialized)
            nativeWindow.Native.Initialize();
    }

    internal void Initialize()
    {
        if (_initialized)
            throw new InvalidOperationException("WindowManager is already initialized.");

        _initialized = true;
        foreach (var window in _windows)
        {
            window.Native.Initialize();
        }
    }
}