using System.Collections.Concurrent;
using System.Numerics;

using Shimakaze.UI.Core;
using Shimakaze.UI.Input.EventArgs;

using Silk.NET.Input;

namespace Shimakaze.UI.Input;

public sealed class InputManager : IDisposable
{
    private IInputContext? _inputContext;
    private readonly ConcurrentDictionary<Key, KeyboardManagerContext> _states = new();

    private bool _disposedValue;

    /// <summary>
    /// 首次按键重复延迟（秒），默认 0.5 秒
    /// </summary>
    public TimeSpan InitialRepeatDelay { get; set; } = TimeSpan.FromSeconds(0.5);

    /// <summary>
    /// 后续按键重复间隔（秒），默认 0.05 秒
    /// </summary>
    public TimeSpan RepeatInterval { get; set; } = TimeSpan.FromSeconds(0.05);

    public event UIEventHandler<InputManager, InputDeviceConnectionChangedEventArgs>? InputDeviceConnectionChanged;

    public event UIEventHandler<InputManager, JoystickAxisMovedEventArgs>? JoystickAxisMoved;
    public event UIEventHandler<InputManager, JoystickHatMovedEventArgs>? JoystickHatMoved;
    public event UIEventHandler<InputManager, JoystickButtonEventArgs>? JoystickButtonUp;
    public event UIEventHandler<InputManager, JoystickButtonEventArgs>? JoystickButtonDown;

    public event UIEventHandler<InputManager, GamepadTriggerMovedEventArgs>? GamepadTriggerMoved;
    public event UIEventHandler<InputManager, GamepadThumbstickMovedEventArgs>? GamepadThumbstickMoved;
    public event UIEventHandler<InputManager, GamepadButtonEventArgs>? GamepadButtonUp;
    public event UIEventHandler<InputManager, GamepadButtonEventArgs>? GamepadButtonDown;

    public event UIEventHandler<InputManager, MouseScrollEventArgs>? MouseScroll;
    public event UIEventHandler<InputManager, MouseMoveEventArgs>? MouseMove;
    public event UIEventHandler<InputManager, MouseClickEventArgs>? MouseDoubleClick;
    public event UIEventHandler<InputManager, MouseClickEventArgs>? MouseClick;
    public event UIEventHandler<InputManager, MouseButtonEventArgs>? MouseUp;
    public event UIEventHandler<InputManager, MouseButtonEventArgs>? MouseDown;

    public event UIEventHandler<InputManager, KeyboardKeyEventArgs>? KeyboardKeyDown;
    public event UIEventHandler<InputManager, KeyboardKeyEventArgs>? KeyboardKeyUp;
    public event UIEventHandler<InputManager, KeyboardKeyPressedEventArgs>? KeyboardKeyPressed;

    public PlatformWindow Window { get; }

    internal InputManager(IInputContextProvider inputContextProvider, PlatformWindow window)
    {
        Window = window;

        if (window.IsInitialized)
            Initialize(inputContextProvider)(window, System.EventArgs.Empty);
        else
            window.Initialize += Initialize(inputContextProvider);
    }

    private UIEventHandler<PlatformWindow> Initialize(IInputContextProvider inputContextProvider) => (window, eventArgs) =>
    {
        window.Update += OnWindowUpdate;
        _inputContext = inputContextProvider.CreateInputContext(window.Native);
        _inputContext.ConnectionChanged += OnInputDeviceConnectionChanged;
        foreach (var device in _inputContext.Devices)
            InitializeInputDevice(device, device.IsConnected);
    };

    private void OnWindowUpdate(PlatformWindow sender, WindowUpdateEventArgs eventArgs)
    {
        var dt = TimeSpan.FromSeconds(eventArgs.DeltaTime);

        foreach (var state in _states.Values)
        {
            var prevTime = state.PressingTime;
            state.PressingTime += dt;
            var curTime = state.PressingTime;

            // 检查是否已经超过初始延迟，未超过则不触发重复
            if (curTime < InitialRepeatDelay)
                continue;

            var prevOver = prevTime - InitialRepeatDelay;
            var curOver = curTime - InitialRepeatDelay;

            var curRep = (int)(curOver / RepeatInterval);
            var prevRep = (int)(prevOver / RepeatInterval);

            if (curRep > prevRep)
                OnKeyboardKeyPressed(state);
        }
    }

    private void InitializeInputDevice(IInputDevice device, bool connected)
    {
        switch (device)
        {
            case IGamepad gamepad when connected:
                gamepad.ButtonDown += OnGamepadButtonDown;
                gamepad.ButtonUp += OnGamepadButtonUp;
                gamepad.ThumbstickMoved += OnGamepadThumbstickMoved;
                gamepad.TriggerMoved += OnGamepadTriggerMoved;
                break;
            case IGamepad gamepad when !connected:
                gamepad.ButtonDown -= OnGamepadButtonDown;
                gamepad.ButtonUp -= OnGamepadButtonUp;
                gamepad.ThumbstickMoved -= OnGamepadThumbstickMoved;
                gamepad.TriggerMoved -= OnGamepadTriggerMoved;
                break;
            case IJoystick joystick when connected:
                joystick.ButtonDown += OnJoystickButtonDown;
                joystick.ButtonUp += OnJoystickButtonUp;
                joystick.AxisMoved += OnJoystickAxisMoved;
                joystick.HatMoved += OnJoystickHatMoved;
                break;
            case IJoystick joystick when !connected:
                joystick.ButtonDown -= OnJoystickButtonDown;
                joystick.ButtonUp -= OnJoystickButtonUp;
                joystick.AxisMoved -= OnJoystickAxisMoved;
                joystick.HatMoved -= OnJoystickHatMoved;
                break;
            case IKeyboard keyboard when connected:
                keyboard.KeyDown += OnKeyboardKeyDown;
                keyboard.KeyUp += OnKeyboardKeyUp;
                break;
            case IKeyboard keyboard when !connected:
                keyboard.KeyDown -= OnKeyboardKeyDown;
                keyboard.KeyUp -= OnKeyboardKeyUp;
                break;
            case IMouse mouse when connected:
                mouse.MouseDown += OnMouseDown;
                mouse.MouseUp += OnMouseUp;
                mouse.Click += OnMouseClick;
                mouse.DoubleClick += OnMouseDoubleClick;
                mouse.MouseMove += OnMouseMove;
                mouse.Scroll += OnMouseScroll;
                break;
            case IMouse mouse when !connected:
                mouse.MouseDown -= OnMouseDown;
                mouse.MouseUp -= OnMouseUp;
                mouse.Click -= OnMouseClick;
                mouse.DoubleClick -= OnMouseDoubleClick;
                mouse.MouseMove -= OnMouseMove;
                mouse.Scroll -= OnMouseScroll;
                break;
        }
    }

    private void OnGamepadButtonDown(IGamepad gamepad, Button button)
    {
        GamepadButtonDown?.Invoke(this, new(gamepad, button));
    }

    private void OnGamepadButtonUp(IGamepad gamepad, Button button)
    {
        GamepadButtonUp?.Invoke(this, new(gamepad, button));
    }

    private void OnGamepadThumbstickMoved(IGamepad gamepad, Thumbstick thumbstick)
    {
        GamepadThumbstickMoved?.Invoke(this, new(gamepad, thumbstick));
    }

    private void OnGamepadTriggerMoved(IGamepad gamepad, Trigger trigger)
    {
        GamepadTriggerMoved?.Invoke(this, new(gamepad, trigger));
    }

    private void OnJoystickButtonDown(IJoystick joystick, Button button)
    {
        JoystickButtonDown?.Invoke(this, new(joystick, button));
    }

    private void OnJoystickButtonUp(IJoystick joystick, Button button)
    {
        JoystickButtonUp?.Invoke(this, new(joystick, button));
    }

    private void OnJoystickAxisMoved(IJoystick joystick, Axis axis)
    {
        JoystickAxisMoved?.Invoke(this, new(joystick, axis));
    }

    private void OnJoystickHatMoved(IJoystick joystick, Hat hat)
    {
        JoystickHatMoved?.Invoke(this, new(joystick, hat));
    }

    private void OnKeyboardKeyDown(IKeyboard keyboard, Key key, int scancode)
    {
        KeyboardManagerContext context = new(keyboard, key, scancode);
        _states.TryAdd(key, context);
        KeyboardKeyDown?.Invoke(this, new(keyboard, key, scancode));
        OnKeyboardKeyPressed(context);
    }

    private void OnKeyboardKeyUp(IKeyboard keyboard, Key key, int scancode)
    {
        _states.Remove(key, out _);
        KeyboardKeyUp?.Invoke(this, new(keyboard, key, scancode));
    }

    private void OnKeyboardKeyPressed(KeyboardManagerContext context)
    {
        KeyboardKeyPressed?.Invoke(this, context.Build());
        context.RepeatCount++;
    }

    private void OnMouseDown(IMouse mouse, MouseButton button)
    {
        MouseDown?.Invoke(this, new(mouse, button));
    }

    private void OnMouseUp(IMouse mouse, MouseButton button)
    {
        MouseUp?.Invoke(this, new(mouse, button));
    }

    private void OnMouseClick(IMouse mouse, MouseButton button, Vector2 position)
    {
        MouseClick?.Invoke(this, new(mouse, button, position));
    }

    private void OnMouseDoubleClick(IMouse mouse, MouseButton button, Vector2 position)
    {
        MouseDoubleClick?.Invoke(this, new(mouse, button, position));
    }

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        MouseMove?.Invoke(this, new(mouse, position));
    }

    private void OnMouseScroll(IMouse mouse, ScrollWheel wheel)
    {
        MouseScroll?.Invoke(this, new(mouse, wheel));
    }

    private void OnInputDeviceConnectionChanged(IInputDevice device, bool connected)
    {
        InitializeInputDevice(device, connected);
        InputDeviceConnectionChanged?.Invoke(this, new(device, connected));
    }

    private void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            _inputContext?.Dispose();
        }

        _disposedValue = true;
    }

    // ~InputManager()
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

    private sealed class KeyboardManagerContext(IKeyboard keyboard, Key key, int scancode)
    {
        public TimeSpan PressingTime { get; set; }
        public int RepeatCount { get; set; }
        public IKeyboard Keyboard { get; } = keyboard;
        public Key Key { get; } = key;
        public int Scancode { get; } = scancode;

        public KeyboardKeyPressedEventArgs Build()
            => new(Keyboard, Key, Scancode, RepeatCount > 0, RepeatCount);
    }
}