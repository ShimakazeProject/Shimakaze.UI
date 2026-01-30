using Shimakaze.UI.Core.Input;

using Silk.NET.Input;
using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public partial class Window : INativeWindow
{
    private readonly WindowManager _windowManager;
    private readonly IWindow _native;

    public bool IsInitialized => _native.IsInitialized;

    private IInputContext InputContext => field ??= _windowManager.CreateInputContext(_native);

    public Window()
    {
        _windowManager = Application.Instance.WindowManager;
        _native = _windowManager.CreateNativeWindow();
    }

    protected internal void OnInitialize()
    {
        _native.Initialize();
        InitializeInput();
    }

    private void InitializeInput()
    {
        InputContext.ConnectionChanged += OnInputDeviceConnectionChanged;
        foreach (var device in InputContext.Devices)
            InputDeviceConnectionChanged(device, device.IsConnected);
    }

    private void InputDeviceConnectionChanged(IInputDevice device, bool connected)
    {
        switch (device)
        {
            case IGamepad gamepad when connected:
                gamepad.ButtonDown += OnButtonDown;
                gamepad.ButtonUp += OnButtonUp;
                gamepad.ThumbstickMoved += OnThumbstickMoved;
                gamepad.TriggerMoved += OnTriggerMoved;
                break;
            case IGamepad gamepad when !connected:
                gamepad.ButtonDown -= OnButtonDown;
                gamepad.ButtonUp -= OnButtonUp;
                gamepad.ThumbstickMoved -= OnThumbstickMoved;
                gamepad.TriggerMoved -= OnTriggerMoved;
                break;
            case IJoystick joystick when connected:
                joystick.ButtonDown += OnButtonDown;
                joystick.ButtonUp += OnButtonUp;
                joystick.AxisMoved += OnAxisMoved;
                joystick.HatMoved += OnHatMoved;
                break;
            case IJoystick joystick when !connected:
                joystick.ButtonDown -= OnButtonDown;
                joystick.ButtonUp -= OnButtonUp;
                joystick.AxisMoved -= OnAxisMoved;
                joystick.HatMoved -= OnHatMoved;
                break;
            case IKeyboard keyboard when connected:
                keyboard.KeyDown += OnKeyDown;
                keyboard.KeyUp += OnKeyUp;
                break;
            case IKeyboard keyboard when !connected:
                keyboard.KeyDown -= OnKeyDown;
                keyboard.KeyUp -= OnKeyUp;
                break;
            case IMouse mouse when connected:
                mouse.MouseDown += OnMouseDown;
                mouse.MouseUp += OnMouseUp;
                mouse.Click += OnClick;
                mouse.DoubleClick += OnDoubleClick;
                mouse.MouseMove += OnMouseMove;
                mouse.Scroll += OnScroll;
                break;
            case IMouse mouse when !connected:
                mouse.MouseDown -= OnMouseDown;
                mouse.MouseUp -= OnMouseUp;
                mouse.Click -= OnClick;
                mouse.DoubleClick -= OnDoubleClick;
                mouse.MouseMove -= OnMouseMove;
                mouse.Scroll -= OnScroll;
                break;
        }
    }

    IWindow INativeWindow.Native => _native;
}