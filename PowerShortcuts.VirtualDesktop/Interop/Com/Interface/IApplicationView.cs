using System.Runtime.InteropServices;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("372E1D3B-38D3-42E4-A15B-8AB2B178F513")]
internal interface IApplicationView
{
    // IInspectable methods
    void GetIids(out ulong iidCount, out IntPtr iids);
    void GetRuntimeClassName(out IntPtr className);
    void GetTrustLevel(out int trustLevel);
    
    // IApplicationView methods
    void SetFocus();
    void SwitchTo();
    void TryInvokeBack(uint asyncCallback); 
    int GetThumbnailWindow(out IntPtr hwnd);
    void GetMonitor(out uint immersiveMonitor); 
    int GetVisibility(out uint pVisible);
    void SetCloak(uint cloakType, uint unknown);
    void GetPosition(ref Guid refId, out IntPtr lpVoid);
    void SetPosition(uint applicationViewPosition);
    void InsertAfterWindow(IntPtr hwnd);
    void GetExtendedFramePosition(out RECT rect);
    int GetAppUserModelId(out string pId);
    void SetAppUserModelId([MarshalAs(UnmanagedType.LPWStr)] string appId);
    void IsEqualByAppUserModelId([MarshalAs(UnmanagedType.LPWStr)] string appId, out uint result);
    void GetViewState(out uint state);
    void SetViewState(uint state);
    void GetNeediness(out uint neediness);
    int GetLastActivationTimestamp(out ulong timestamp);
    void SetLastActivationTimestamp(ulong timestamp);
    int GetVirtualDesktopId(out Guid guid);
    void SetVirtualDesktopId(ref Guid guid);
    int GetShowInSwitchers(out uint shown);
    void SetShowInSwitchers(uint shown);
    void GetScaleFactor(out uint factor);
    void CanReceiveInput(out bool canReceive);
    void GetCompatibilityPolicyType(out uint policyType); 
    void SetCompatibilityPolicyType(uint policyType);
    void GetSizeConstraints(uint immersiveMonitor, out SIZE minSize, out SIZE maxSize);
    void GetSizeConstraintsForDpi(uint dpi, out SIZE minSize, out SIZE maxSize);
    void SetSizeConstraintsForDpi(ref uint dpi, ref SIZE minSize, ref SIZE maxSize);
    void OnMinSizePreferencesUpdated(IntPtr hwnd);
    void ApplyOperation(uint applicationViewOperation);
    void IsTray(out bool isTray);
    void IsInHighZOrderBand(out bool isInHighZOrderBand);
    void IsSplashScreenPresented(out bool isSplashScreenPresented);
    void Flash();
    void GetRootSwitchableOwner(out IntPtr applicationView);
    void EnumerateOwnershipTree(out IntPtr objectArray);
    void GetEnterpriseId(out string enterpriseId);
    void IsMirrored(out bool isMirrored);
    void GetFrameworkViewType(out uint viewTypeId);
    void GetCanTab(out uint canTab);
    void SetCanTab(uint canTab);
    void GetIsTabbed(out uint isTabbed);
    void SetIsTabbed(uint isTabbed);
    void RefreshCanTab();
    void GetIsOccluded(out uint isOccluded);
    void SetIsOccluded(uint isOccluded);
    void UpdateEngagementFlags(uint flags, uint value);
    void SetForceActiveWindowAppearance(uint forceActive);
    void GetLastActivationFILETIME(out SIZE fileTime);
    void GetPersistingStateName(out string stateName);
}

[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public int Left, Top, Right, Bottom;
}

[StructLayout(LayoutKind.Sequential)]
public struct SIZE
{
    public int cx, cy;
}
