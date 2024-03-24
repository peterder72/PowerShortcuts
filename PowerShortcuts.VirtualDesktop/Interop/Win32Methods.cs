using System.Runtime.InteropServices;
using System.Text;

namespace PowerShortcuts.VirtualDesktop.Interop;

internal static class Win32Methods
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetWindowTextLength(IntPtr hWnd);
}