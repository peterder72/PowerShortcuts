namespace PowerShortcuts.VirtualDesktop.Interface;

public interface IWindowManager: IDisposable
{
    public IntPtr GetWindowInFocus();
    public string GetWindowTitle(IntPtr hwnd);
}