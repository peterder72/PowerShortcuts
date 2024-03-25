namespace PowerShortcuts.VirtualDesktop.Interface;

public interface IVirtualDesktopManager
{
    public int GetDesktopCount();

    public Guid GetDesktopIdByNumber(int number);

    public int GetDesktopNumberById(Guid desktopId);

    public Guid GetWindowDesktopId(IntPtr hwnd);

    public int GetWindowDesktopNumber(IntPtr hwnd);

    public bool IsWindowOnCurrentVirtualDesktop(IntPtr hwnd);

    public bool MoveWindowToDesktopNumber(IntPtr hwnd, int desktopNumber);

    public bool GoToDesktopNumber(int desktopNumber);

    public bool RegisterPostMessageHook(IntPtr listenerHwnd, uint messageOffset);

    public bool UnregisterPostMessageHook(IntPtr listenerHwnd);

    public bool IsPinnedWindow(IntPtr hwnd);

    public bool PinWindow(IntPtr hwnd);

    public bool UnPinWindow(IntPtr hwnd);

    public bool IsPinnedApp(IntPtr hwnd);

    public bool PinApp(IntPtr hwnd);

    public bool UnPinApp(IntPtr hwnd);

    public bool IsWindowOnDesktopNumber(IntPtr hwnd, int desktopNumber);

    public bool CreateDesktop();

    public bool RemoveDesktop(int removeDesktopNumber, int fallbackDesktopNumber);

    public bool SetDesktopName(int desktopNumber, string name);

    public string GetDesktopName(int desktopNumber, ulong bufferLength = 1024);
}