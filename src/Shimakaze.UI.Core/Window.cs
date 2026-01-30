using System.ComponentModel;

using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public partial class Window : INativeWindow
{
    private readonly IWindow _native;
    IWindow INativeWindow.Native => _native;

    private bool _disposedValue;

    public event UIEventHandler<Window, WindowMoveEventArgs>? Move;
    public event UIEventHandler<Window, WindowStateChangedEventArgs>? StateChanged;
    public event UIEventHandler<Window, WindowFocusChangedEventArgs>? FocusChanged;

    public event UIEventHandler<Window, WindowResizeEventArgs>? Resize;
    public event UIEventHandler<Window, WindowResizeEventArgs>? FramebufferResize;

    public event UIEventHandler<Window>? Initialize;
    public event UIEventHandler<Window, WindowUpdateEventArgs>? Update;
    public event UIEventHandler<Window, WindowUpdateEventArgs>? Render;
    public event UIEventHandler<Window, CancelEventArgs>? Closing;

    public event UIEventHandler<Window, FileDropEventArgs>? FileDrop;

    public bool IsInitialized => _native.IsInitialized;

    public Window()
    {
        _native = Application.Instance.CreateNativeWindow();
        _native.Move += OnMove;
        _native.StateChanged += OnStateChanged;
        _native.FileDrop += OnFileDrop;
        _native.Resize += OnResize;
        _native.FramebufferResize += OnFramebufferResize;
        _native.Closing += () => OnClosing(false);
        _native.FocusChanged += OnFocusChanged;
        _native.Load += OnInitialize;
        _native.Update += OnUpdate;
        _native.Render += OnRender;
        Application.Instance.WindowManager.Register(this);
    }

    protected virtual void OnMove(Vector2D<int> position)
    {
        Move?.Invoke(this, new(position));
    }

    protected virtual void OnStateChanged(WindowState windowState)
    {
        StateChanged?.Invoke(this, new(windowState));
    }

    protected virtual void OnFocusChanged(bool focused)
    {
        FocusChanged?.Invoke(this, new(focused));
    }

    protected virtual void OnResize(Vector2D<int> newSize)
    {
        Resize?.Invoke(this, new(newSize));
    }

    protected virtual void OnFramebufferResize(Vector2D<int> newSize)
    {
        FramebufferResize?.Invoke(this, new(newSize));
    }

    protected virtual void OnInitialize()
    {
        Initialize?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnUpdate(double deltaTime)
    {
        Update?.Invoke(this, new(deltaTime));
    }

    protected virtual void OnRender(double deltaTime)
    {
        Render?.Invoke(this, new(deltaTime));
    }

    protected virtual void OnClosing(bool cancel)
    {
        CancelEventArgs cancelEventArgs = new(cancel);
        Closing?.Invoke(this, cancelEventArgs);
        if (cancelEventArgs.Cancel)
            _native.IsClosing = false;
    }

    protected virtual void OnFileDrop(string[] paths)
    {
        FileDrop?.Invoke(this, new(paths));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            _native.Dispose();
        }

        _disposedValue = true;
    }

    // ~Window()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}