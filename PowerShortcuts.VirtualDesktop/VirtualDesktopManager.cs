using System;
using System.Runtime.InteropServices;
using System.Text;
using PowerShortcuts.VirtualDesktop;

public sealed class VirtualDesktopManager
{
    public int GetCurrentDesktopNumber() => RustInterop.GetCurrentDesktopNumber();

    public int GetDesktopCount() => RustInterop.GetDesktopCount();

    public Guid GetDesktopIdByNumber(int number) => RustInterop.GetDesktopIdByNumber(number);

    public int GetDesktopNumberById(Guid desktopId) => RustInterop.GetDesktopNumberById(desktopId);

    public Guid GetWindowDesktopId(IntPtr hwnd) => RustInterop.GetWindowDesktopId(hwnd);

    public int GetWindowDesktopNumber(IntPtr hwnd) => RustInterop.GetWindowDesktopNumber(hwnd);

    public bool IsWindowOnCurrentVirtualDesktop(IntPtr hwnd) => RustInterop.IsWindowOnCurrentVirtualDesktop(hwnd) != 0;

    public bool MoveWindowToDesktopNumber(IntPtr hwnd, int desktopNumber) => RustInterop.MoveWindowToDesktopNumber(hwnd, desktopNumber) != 0;

    public bool GoToDesktopNumber(int desktopNumber) => RustInterop.GoToDesktopNumber(desktopNumber) != 0;

    public bool SetDesktopName(int desktopNumber, string name)
    {
        var nameBytes = Encoding.UTF8.GetBytes(name + "\0");
        var ptr = Marshal.AllocHGlobal(nameBytes.Length);
        try
        {
            Marshal.Copy(nameBytes, 0, ptr, nameBytes.Length);
            return RustInterop.SetDesktopName(desktopNumber, ptr) != 0;
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public string GetDesktopName(int desktopNumber, ulong bufferLength = 1024)
    {
        var buffer = new byte[bufferLength];
        var ptr = Marshal.AllocHGlobal((int)bufferLength);
        try
        {
            if (RustInterop.GetDesktopName(desktopNumber, ptr, bufferLength) != 0)
            {
                Marshal.Copy(ptr, buffer, 0, (int)bufferLength);
                return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            }
            return string.Empty;
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public bool RegisterPostMessageHook(IntPtr listenerHwnd, uint messageOffset) => RustInterop.RegisterPostMessageHook(listenerHwnd, messageOffset) != 0;

    public bool UnregisterPostMessageHook(IntPtr listenerHwnd) => RustInterop.UnregisterPostMessageHook(listenerHwnd) != 0;

    public bool IsPinnedWindow(IntPtr hwnd) => RustInterop.IsPinnedWindow(hwnd) != 0;

    public bool PinWindow(IntPtr hwnd) => RustInterop.PinWindow(hwnd) != 0;

    public bool UnPinWindow(IntPtr hwnd) => RustInterop.UnPinWindow(hwnd) != 0;

    public bool IsPinnedApp(IntPtr hwnd) => RustInterop.IsPinnedApp(hwnd) != 0;

    public bool PinApp(IntPtr hwnd) => RustInterop.PinApp(hwnd) != 0;

    public bool UnPinApp(IntPtr hwnd) => RustInterop.UnPinApp(hwnd) != 0;

    public bool IsWindowOnDesktopNumber(IntPtr hwnd, int desktopNumber) => RustInterop.IsWindowOnDesktopNumber(hwnd, desktopNumber) != 0;

    public bool CreateDesktop() => RustInterop.CreateDesktop() != 0;

    public bool RemoveDesktop(int removeDesktopNumber, int fallbackDesktopNumber) => RustInterop.RemoveDesktop(removeDesktopNumber, fallbackDesktopNumber) != 0;
}
