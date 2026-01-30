using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public partial class Window : INativeWindow
{
    private readonly IWindow _native;

    public bool IsInitialized => _native.IsInitialized;


    public Window()
    {
        _native = Application.Instance.CreateNativeWindow();
    }

    protected internal virtual void OnInitialize()
    {
        Application.Instance.WindowManager.Register(this);
        _native.Initialize();
    }

    protected internal virtual void OnClose()
    {
        _native.Close();
        Application.Instance.WindowManager.Unregister(this);
    }

    IWindow INativeWindow.Native => _native;
}