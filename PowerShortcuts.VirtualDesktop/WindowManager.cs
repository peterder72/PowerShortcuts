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
    private readonly ComReconnectWrapper<ApplicationViewCollection> m_ApplicationViewCollection;
    private readonly DisposableStack m_Disposables = new();
    private readonly ILogger<WindowManager> m_Logger;

    private const int NotFoundCode = -2147023728;

    public WindowManager(ILogger<WindowManager> logger)
    {
        m_Logger = logger;
        m_ApplicationViewCollection = new ComReconnectWrapper<ApplicationViewCollection>(CreateApplicationViewCollection, OnComReconnect);
    }
    
    private void OnComReconnect()
    {
        m_Logger.LogWarning("COM connection dropped, attempting to reconnect");
    }

    private ApplicationViewCollection CreateApplicationViewCollection()
    {
        m_Logger.LogInformation("Establishing COM connection");
        
        m_Disposables.DisposeItems();
        
        var serviceProvider = ServiceProvider.Create();
        m_Disposables.Push(serviceProvider);

        var viewCollectionContainer = serviceProvider.QueryService<IApplicationViewCollection>(typeof(IApplicationViewCollection).GUID);
        m_Disposables.Push(viewCollectionContainer);

        return new ApplicationViewCollection(viewCollectionContainer.Value);
    }
    
    public string? GetWindowTitle(IntPtr hwnd)
    {
        var length = Win32Methods.GetWindowTextLength(hwnd) + 1;
        var title = new StringBuilder(length);
        
        var res = Win32Methods.GetWindowText(hwnd, title, length);

        if (res > 0) return title.ToString();
        
        WindowManagerLogging.LogUnsuccessfulGetWindowTitle(m_Logger, res);
        return null;

    }

    public IntPtr GetWindowInFocus()
    {
        try
        {
            var view = m_ApplicationViewCollection.Execute(x => x.GetViewInFocus());
            
            return view.GetThumbnailWindow();
        } 
        // No window in focus
        catch (COMException e) when (e.HResult == NotFoundCode)
        {
            return IntPtr.Zero;
        }
    }
    
    public void Dispose()
    {
        m_Disposables.Dispose();
    }
}