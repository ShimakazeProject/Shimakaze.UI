
using System.Numerics;

using Silk.NET.Input;

namespace Shimakaze.UI.Core;

partial class Window
{
    protected virtual void OnInputDeviceConnectionChanged(IInputDevice device, bool connected)
    {
        InputDeviceConnectionChanged(device, connected);
    }
    
    protected virtual void OnAxisMoved(IJoystick joystick, Axis axis)
    {
    }

    protected virtual void OnHatMoved(IJoystick joystick, Hat hat)
    {
    }

    protected virtual void OnTriggerMoved(IGamepad gamepad, Trigger trigger)
    {
    }

    protected virtual void OnThumbstickMoved(IGamepad gamepad, Thumbstick thumbstick)
    {
    }

    protected virtual void OnButtonUp(IInputDevice gamepad, Button button)
    {
    }

    protected virtual void OnButtonDown(IInputDevice gamepad, Button button)
    {
    }

    protected virtual void OnScroll(IMouse mouse, ScrollWheel wheel)
    {
    }

    protected virtual void OnMouseMove(IMouse mouse, Vector2 vector)
    {
    }

    protected virtual void OnDoubleClick(IMouse mouse, MouseButton button, Vector2 vector)
    {
    }

    protected virtual void OnClick(IMouse mouse, MouseButton button, Vector2 vector)
    {
    }

    protected virtual void OnMouseUp(IMouse mouse, MouseButton button)
    {
    }

    protected virtual void OnMouseDown(IMouse mouse, MouseButton button)
    {
    }

    protected virtual void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
    {
    }

    protected virtual void OnKeyUp(IKeyboard keyboard, Key key, int arg3)
    {
    }
}