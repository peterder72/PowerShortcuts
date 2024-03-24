using System.Runtime.InteropServices;

namespace PowerShortcuts.VirtualDesktop.Interop;

internal static class RustVdaInterop
{
    private const string DllName = "Native/VirtualDesktopAccessor.dll";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetCurrentDesktopNumber();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetDesktopCount();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Guid GetDesktopIdByNumber(int number);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetDesktopNumberById(Guid desktopId);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Guid GetWindowDesktopId(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetWindowDesktopNumber(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IsWindowOnCurrentVirtualDesktop(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MoveWindowToDesktopNumber(IntPtr hwnd, int desktopNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GoToDesktopNumber(int desktopNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetDesktopName(int desktopNumber, IntPtr inNamePtr);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetDesktopName(int desktopNumber, IntPtr outUtf8Ptr, ulong outUtf8Len);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int RegisterPostMessageHook(IntPtr listenerHwnd, uint messageOffset);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int UnregisterPostMessageHook(IntPtr listenerHwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IsPinnedWindow(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int PinWindow(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int UnPinWindow(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IsPinnedApp(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int PinApp(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int UnPinApp(IntPtr hwnd);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IsWindowOnDesktopNumber(IntPtr hwnd, int desktopNumber);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int CreateDesktop();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int RemoveDesktop(int removeDesktopNumber, int fallbackDesktopNumber);
}
