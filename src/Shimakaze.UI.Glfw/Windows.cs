
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Microsoft.Win32;

using Silk.NET.Windowing;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Shimakaze.UI.Glfw;

[SupportedOSPlatform("windows")]
internal static class Windows
{
    public static void Register(IWindow window)
    {
        if (!window.IsInitialized)
        {
            window.Load += () => Register(window);
            return;
        }

        if (window is not { Native.Win32.Hwnd: { } hWnd })
            return;

        HWND hwnd = (HWND)hWnd;

        if (OperatingSystem.IsWindowsVersionAtLeast(5))
        {
            unsafe
            {
                var threadId = Win32.GetWindowThreadProcessId(hwnd);
                Win32.SetWindowsHookEx(
                    WINDOWS_HOOK_ID.WH_CALLWNDPROC,
                    &CallWndProc,
                    null,
                    threadId);
            }
        }

        SetWindowTitlebar(hwnd, AppThemeIsDark());
    }

    private static bool AppThemeIsDark()
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        if (key is null)
            return false;

        if (key.GetValueKind("AppsUseLightTheme") is not RegistryValueKind.DWord)
            return false;

        var value = key.GetValue("AppsUseLightTheme");

        return value is 0;
    }

    private static void SetWindowTitlebar(HWND hwnd, bool isDark)
    {

        if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22621))
        {
            var v = DWM_SYSTEMBACKDROP_TYPE.DWMSBT_AUTO;
            Win32.DwmSetWindowAttribute(
                hwnd,
                DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref v, 1)));
        }

        if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
        {
            var v = isDark ? 1 : 0;
            Win32.DwmSetWindowAttribute(
                hwnd,
                DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref v, 1)));
        }
        else if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18362))
        {
            var v = isDark ? 1 : 0;
            Win32.DwmSetWindowAttribute(
                hwnd,
                (DWMWINDOWATTRIBUTE)19,
                MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref v, 1)));
        }
        else if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763))
        {
            var v = isDark ? 2 : 0;
            Win32.DwmSetWindowAttribute(
                hwnd,
                (DWMWINDOWATTRIBUTE)19,
                MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref v, 1)));
        }
    }

    [SupportedOSPlatform("windows5.0")]
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static unsafe LRESULT CallWndProc(int nCode, WPARAM wParam, LPARAM lParam)
    {
        if (nCode >= 0)
        {
            CWPSTRUCT* pMsg = (CWPSTRUCT*)lParam.Value;

            // 检查特定窗口的消息
            if (pMsg->message == Win32.WM_SETTINGCHANGE)
            {
                SetWindowTitlebar(pMsg->hwnd, AppThemeIsDark());
            }
        }

        if (nCode == Win32.WM_SETTINGCHANGE)
        {

        }

        return Win32.CallNextHookEx(null, nCode, wParam, lParam);
    }
}