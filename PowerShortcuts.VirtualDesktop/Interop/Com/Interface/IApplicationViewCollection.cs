using System.Runtime.InteropServices;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("1841C6D7-4F9D-42C0-AF41-8747538F10E5")]
internal interface IApplicationViewCollection
{
    void GetViews(out IObjectArray objectArray);
    int GetViewsByZOrder(out IObjectArray objectArray);
    void GetViewsByAppUserModelId([MarshalAs(UnmanagedType.LPWStr)] string appUserModelId, out IObjectArray objectArray);
    int GetViewForHwnd(IntPtr hwnd, out IApplicationView view);
    void GetViewForApplication(uint immersiveApplication, out IApplicationView view);
    void GetViewForAppUserModelId([MarshalAs(UnmanagedType.LPWStr)] string appUserModelId, out IApplicationView view);
    int GetViewInFocus(out IApplicationView view);
    void Unknown1(out IApplicationView view);
    void RefreshCollection();
    void RegisterForApplicationViewChanges(uint viewChangeListener, out uint cookie);
    void UnregisterForApplicationViewChanges(uint cookie);
}
