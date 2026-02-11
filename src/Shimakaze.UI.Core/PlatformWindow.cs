using System.ComponentModel;
using System.Drawing;

using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public partial class PlatformWindow
{
    public readonly IWindow Native;

    private bool _disposedValue;

    public virtual SizeF Size
    {
        get => new(Native.Size.X, Native.Size.Y);
        set => Native.Size = new((int)value.Width, (int)value.Height);
    }

    public event UIEventHandler<PlatformWindow, WindowMoveEventArgs>? Move;
    public event UIEventHandler<PlatformWindow, WindowStateChangedEventArgs>? StateChanged;
    public event UIEventHandler<PlatformWindow, WindowFocusChangedEventArgs>? FocusChanged;

    public event UIEventHandler<PlatformWindow, WindowResizeEventArgs>? Resize;
    public event UIEventHandler<PlatformWindow, WindowResizeEventArgs>? FramebufferResize;

    public event UIEventHandler<PlatformWindow>? Initialize;
    public event UIEventHandler<PlatformWindow, WindowUpdateEventArgs>? Update;
    public event UIEventHandler<PlatformWindow, WindowUpdateEventArgs>? Render;
    public event UIEventHandler<PlatformWindow, CancelEventArgs>? Closing;
    public event UIEventHandler<PlatformWindow>? Closed;

    public event UIEventHandler<PlatformWindow, FileDropEventArgs>? FileDrop;

    public bool IsInitialized => Native.IsInitialized;

    public PlatformWindow()
    {
        Native = Application.Instance.CreateNativeWindow();
        Native.Move += OnMove;
        Native.StateChanged += OnStateChanged;
        Native.FileDrop += OnFileDrop;
        Native.Resize += OnResize;
        Native.FramebufferResize += OnFramebufferResize;
        Native.Closing += () => OnClosing(false);
        Native.FocusChanged += OnFocusChanged;
        Native.Load += OnInitialize;
        Native.Update += OnUpdate;
        Native.Render += OnRender;
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
            Native.IsClosing = false;

        if (Native.IsClosing)
            Closed?.Invoke(this, EventArgs.Empty);
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
            Native.Dispose();
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