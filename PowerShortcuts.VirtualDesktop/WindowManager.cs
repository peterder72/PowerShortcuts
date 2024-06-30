using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using PowerShortcuts.Utils;
using PowerShortcuts.VirtualDesktop.Interface;
using PowerShortcuts.VirtualDesktop.Interop;
using PowerShortcuts.VirtualDesktop.Interop.Com.Implementation;
using PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

namespace PowerShortcuts.VirtualDesktop;

internal sealed class WindowManager: IWindowManager
{
    private readonly ApplicationViewCollection m_ApplicationViewCollection;
    private readonly DisposableStack m_Disposables = new();
    private readonly ILogger<WindowManager> m_Logger;

    public WindowManager(ILogger<WindowManager> logger)
    {
        var serviceProvider = ServiceProvider.Create();
        m_Disposables.Push(serviceProvider);
        m_Logger = logger;

        var viewCollectionContainer = serviceProvider.QueryService<IApplicationViewCollection>(typeof(IApplicationViewCollection).GUID);
        m_Disposables.Push(viewCollectionContainer);

        var viewCollection = new ApplicationViewCollection(viewCollectionContainer.Value);

        m_ApplicationViewCollection = viewCollection;
    }

    public string? GetWindowTitle(IntPtr hwnd)
    {
        var length = Win32Methods.GetWindowTextLength(hwnd) + 1;
        var title = new StringBuilder(length);
        
        var res = Win32Methods.GetWindowText(hwnd, title, length);

        if (res <= 0)
        {
            WindowManagerLogging.LogUnsuccessfulGetWindowTitle(m_Logger, res);
            return null;
        }
        
        return title.ToString();
    }

    public IntPtr GetWindowInFocus()
    {
        try
        {
            var view = m_ApplicationViewCollection.GetViewInFocus();
            return view.GetThumbnailWindow();
        } 
        // No window in focus
        catch (COMException e) when (e.HResult == -2147023728)
        {
            return IntPtr.Zero;
        }
    }

    public void Dispose()
    {
        m_Disposables.Dispose();
    }
}