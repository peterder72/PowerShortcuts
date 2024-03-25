using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using PowerShortcuts.Host;
using PowerShortcuts.VirtualDesktop.Interface;
using PowerShortcuts.VirtualDesktop.Interop;
using PowerShortcuts.VirtualDesktop.Interop.Com;
using PowerShortcuts.VirtualDesktop.Interop.Com.Implementation;
using PowerShortcuts.VirtualDesktop.Interop.Com.Interface;
using IServiceProvider = PowerShortcuts.VirtualDesktop.Interop.Com.Interface.IServiceProvider;

namespace PowerShortcuts.VirtualDesktop;

internal sealed class WindowManager: IWindowManager
{
    private readonly ApplicationViewCollection m_ApplicationViewCollection;
    private readonly DisposableStack m_Disposables = new();

    public WindowManager()
    {
        var serviceProvider = ServiceProvider.Create();
        m_Disposables.Push(serviceProvider);

        var viewCollectionContainer = serviceProvider.QueryService<IApplicationViewCollection>(typeof(IApplicationViewCollection).GUID);
        m_Disposables.Push(viewCollectionContainer);

        var viewCollection = new ApplicationViewCollection(viewCollectionContainer.Value);

        m_ApplicationViewCollection = viewCollection;
    }

    public string GetWindowTitle(IntPtr hwnd)
    {
        var length = Win32Methods.GetWindowTextLength(hwnd) + 1;
        var title = new StringBuilder(length);
        
        var res = Win32Methods.GetWindowText(hwnd, title, length);

        if (res <= 0)
        {
            throw new Exception($"Unsuccessfull win32 getwindowtext call: res={res}");
        }
        
        return title.ToString();
    }

    public IntPtr GetWindowInFocus()
    {
        var view = m_ApplicationViewCollection.GetViewInFocus();
        return view.GetThumbnailWindow();
    }

    public void Dispose()
    {
        m_Disposables.Dispose();
    }
}