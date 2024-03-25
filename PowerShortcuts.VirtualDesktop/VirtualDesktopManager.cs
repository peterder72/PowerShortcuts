using System.Runtime.InteropServices;
using System.Text;
using PowerShortcuts.VirtualDesktop.Interface;
using PowerShortcuts.VirtualDesktop.Interop;

namespace PowerShortcuts.VirtualDesktop;

internal sealed class VirtualDesktopManager: IVirtualDesktopManager
{
    public int GetCurrentDesktopNumber() =>
        RustVdaInterop.GetCurrentDesktopNumber();

    public int GetDesktopCount() =>
        RustVdaInterop.GetDesktopCount();

    public Guid GetDesktopIdByNumber(int number) =>
        RustVdaInterop.GetDesktopIdByNumber(number);

    public int GetDesktopNumberById(Guid desktopId) =>
        RustVdaInterop.GetDesktopNumberById(desktopId);

    public Guid GetWindowDesktopId(IntPtr hwnd) =>
        RustVdaInterop.GetWindowDesktopId(hwnd);

    public int GetWindowDesktopNumber(IntPtr hwnd) =>
        RustVdaInterop.GetWindowDesktopNumber(hwnd);

    public bool IsWindowOnCurrentVirtualDesktop(IntPtr hwnd) =>
        RustVdaInterop.IsWindowOnCurrentVirtualDesktop(hwnd) != 0;

    public bool MoveWindowToDesktopNumber(IntPtr hwnd, int desktopNumber) =>
        RustVdaInterop.MoveWindowToDesktopNumber(hwnd, desktopNumber) != 0;

    public bool GoToDesktopNumber(int desktopNumber) =>
        RustVdaInterop.GoToDesktopNumber(desktopNumber) != 0;


    public bool RegisterPostMessageHook(IntPtr listenerHwnd, uint messageOffset) =>
        RustVdaInterop.RegisterPostMessageHook(listenerHwnd, messageOffset) != 0;

    public bool UnregisterPostMessageHook(IntPtr listenerHwnd) =>
        RustVdaInterop.UnregisterPostMessageHook(listenerHwnd) != 0;

    public bool IsPinnedWindow(IntPtr hwnd) =>
        RustVdaInterop.IsPinnedWindow(hwnd) != 0;

    public bool PinWindow(IntPtr hwnd) =>
        RustVdaInterop.PinWindow(hwnd) != 0;

    public bool UnPinWindow(IntPtr hwnd) =>
        RustVdaInterop.UnPinWindow(hwnd) != 0;

    public bool IsPinnedApp(IntPtr hwnd) =>
        RustVdaInterop.IsPinnedApp(hwnd) != 0;

    public bool PinApp(IntPtr hwnd) =>
        RustVdaInterop.PinApp(hwnd) != 0;

    public bool UnPinApp(IntPtr hwnd) =>
        RustVdaInterop.UnPinApp(hwnd) != 0;

    public bool IsWindowOnDesktopNumber(IntPtr hwnd, int desktopNumber) =>
        RustVdaInterop.IsWindowOnDesktopNumber(hwnd, desktopNumber) != 0;

    public bool CreateDesktop() =>
        RustVdaInterop.CreateDesktop() != 0;

    public bool RemoveDesktop(int removeDesktopNumber, int fallbackDesktopNumber) =>
        RustVdaInterop.RemoveDesktop(removeDesktopNumber, fallbackDesktopNumber) != 0;

    public bool SetDesktopName(int desktopNumber, string name)
    {
        var nameBytes = Encoding.UTF8.GetBytes(name + "\0");
        var ptr = Marshal.AllocHGlobal(nameBytes.Length);
        try
        {
            Marshal.Copy(nameBytes, 0, ptr, nameBytes.Length);
            return RustVdaInterop.SetDesktopName(desktopNumber, ptr) != 0;
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
            if (RustVdaInterop.GetDesktopName(desktopNumber, ptr, bufferLength) != 0)
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
}