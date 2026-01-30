
using System.Numerics;

using Shimakaze.UI.Core;

using Silk.NET.Input;

namespace Shimakaze.UI.Input;

public sealed class WindowInputManager(
    IInputContextProvider inputContextProvider,
    INativeWindow window)
{ 
    private IInputContext InputContext => field ??= inputContextProvider.CreateInputContext(window.Native);

    public void OnInitialize()
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

    internal void OnInputDeviceConnectionChanged(IInputDevice device, bool connected)
    {
        InputDeviceConnectionChanged(device, connected);
    }
    
    internal void OnAxisMoved(IJoystick joystick, Axis axis)
    {
    }

    internal void OnHatMoved(IJoystick joystick, Hat hat)
    {
    }

    internal void OnTriggerMoved(IGamepad gamepad, Trigger trigger)
    {
    }

    internal void OnThumbstickMoved(IGamepad gamepad, Thumbstick thumbstick)
    {
    }

    internal void OnButtonUp(IInputDevice gamepad, Button button)
    {
    }

    internal void OnButtonDown(IInputDevice gamepad, Button button)
    {
    }

    internal void OnScroll(IMouse mouse, ScrollWheel wheel)
    {
    }

    internal void OnMouseMove(IMouse mouse, Vector2 vector)
    {
    }

    internal void OnDoubleClick(IMouse mouse, MouseButton button, Vector2 vector)
    {
    }

    internal void OnClick(IMouse mouse, MouseButton button, Vector2 vector)
    {
    }

    internal void OnMouseUp(IMouse mouse, MouseButton button)
    {
    }

    internal void OnMouseDown(IMouse mouse, MouseButton button)
    {
    }

    internal void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
    {
    }

    internal void OnKeyUp(IKeyboard keyboard, Key key, int arg3)
    {
    }
}