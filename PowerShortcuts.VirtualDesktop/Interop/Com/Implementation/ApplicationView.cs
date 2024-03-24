using System.Runtime.InteropServices;
using PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Implementation;

internal class ApplicationView
{
    private IApplicationView m_ApplicationView;

    public ApplicationView(IApplicationView applicationView)
    {
        this.m_ApplicationView = applicationView ?? throw new ArgumentNullException(nameof(applicationView));
    }

    public void SetFocus()
    {
        m_ApplicationView.SetFocus();
    }

    public void SwitchTo()
    {
        m_ApplicationView.SwitchTo();
    }

    public void TryInvokeBack(uint asyncCallback)
    {
        m_ApplicationView.TryInvokeBack(asyncCallback);
    }

    public IntPtr GetThumbnailWindow()
    {
        m_ApplicationView.GetThumbnailWindow(out var hwnd);
        return hwnd;
    }

    public uint GetVisibility()
    {
        m_ApplicationView.GetVisibility(out var pVisible);
        return pVisible;
    }

    public void SetCloak(uint cloakType, uint unknown)
    {
        m_ApplicationView.SetCloak(cloakType, unknown);
    }

    public void GetPosition(ref Guid refId, out IntPtr lpVoid)
    {
        m_ApplicationView.GetPosition(ref refId, out lpVoid);
    }

    public void SetPosition(uint applicationViewPosition)
    {
        m_ApplicationView.SetPosition(applicationViewPosition);
    }

    public void InsertAfterWindow(IntPtr hwnd)
    {
        m_ApplicationView.InsertAfterWindow(hwnd);
    }

    public RECT GetExtendedFramePosition()
    {
        m_ApplicationView.GetExtendedFramePosition(out var rect);
        return rect;
    }

    public string GetAppUserModelId()
    {
        m_ApplicationView.GetAppUserModelId(out var pId);
        return pId;
    }

    public void SetAppUserModelId(string appId)
    {
        m_ApplicationView.SetAppUserModelId(appId);
    }

    public uint IsEqualByAppUserModelId(string appId)
    {
        m_ApplicationView.IsEqualByAppUserModelId(appId, out var result);
        return result;
    }

    public uint GetViewState()
    {
        m_ApplicationView.GetViewState(out var state);
        return state;
    }

    public void SetViewState(uint state)
    {
        m_ApplicationView.SetViewState(state);
    }

    public uint GetNeediness()
    {
        m_ApplicationView.GetNeediness(out var neediness);
        return neediness;
    }

    public ulong GetLastActivationTimestamp()
    {
        m_ApplicationView.GetLastActivationTimestamp(out var timestamp);
        return timestamp;
    }

    public void SetLastActivationTimestamp(ulong timestamp)
    {
        m_ApplicationView.SetLastActivationTimestamp(timestamp);
    }

    public Guid GetVirtualDesktopId()
    {
        m_ApplicationView.GetVirtualDesktopId(out var guid);
        return guid;
    }

    public void SetVirtualDesktopId(Guid guid)
    {
        m_ApplicationView.SetVirtualDesktopId(ref guid);
    }

    public uint GetShowInSwitchers()
    {
        m_ApplicationView.GetShowInSwitchers(out var shown);
        return shown;
    }

    public void SetShowInSwitchers(uint shown)
    {
        m_ApplicationView.SetShowInSwitchers(shown);
    }

    public uint GetScaleFactor()
    {
        m_ApplicationView.GetScaleFactor(out var factor);
        return factor;
    }

    public bool CanReceiveInput()
    {
        m_ApplicationView.CanReceiveInput(out var canReceive);
        return canReceive;
    }

    public uint GetCompatibilityPolicyType()
    {
        m_ApplicationView.GetCompatibilityPolicyType(out var policyType);
        return policyType;
    }

    public void SetCompatibilityPolicyType(uint policyType)
    {
        m_ApplicationView.SetCompatibilityPolicyType(policyType);
    }

    public (SIZE minSize, SIZE maxSize) GetSizeConstraints(uint immersiveMonitor)
    {
        m_ApplicationView.GetSizeConstraints(immersiveMonitor, out var minSize, out var maxSize);
        return (minSize, maxSize);
    }

    public (SIZE minSize, SIZE maxSize) GetSizeConstraintsForDpi(uint dpi)
    {
        m_ApplicationView.GetSizeConstraintsForDpi(dpi, out var minSize, out var maxSize);
        return (minSize, maxSize);
    }

    public void SetSizeConstraintsForDpi(ref uint dpi, ref SIZE minSize, ref SIZE maxSize)
    {
        m_ApplicationView.SetSizeConstraintsForDpi(ref dpi, ref minSize, ref maxSize);
    }

    public void OnMinSizePreferencesUpdated(IntPtr hwnd)
    {
        m_ApplicationView.OnMinSizePreferencesUpdated(hwnd);
    }

    public void ApplyOperation(uint applicationViewOperation)
    {
        m_ApplicationView.ApplyOperation(applicationViewOperation);
    }

    public bool IsTray()
    {
        m_ApplicationView.IsTray(out var isTray);
        return isTray;
    }

    public bool IsInHighZOrderBand()
    {
        m_ApplicationView.IsInHighZOrderBand(out var isInHighZOrderBand);
        return isInHighZOrderBand;
    }

    public bool IsSplashScreenPresented()
    {
        m_ApplicationView.IsSplashScreenPresented(out var isSplashScreenPresented);
        return isSplashScreenPresented;
    }

    public void Flash()
    {
        m_ApplicationView.Flash();
    }

    public ApplicationView GetRootSwitchableOwner()
    {
        m_ApplicationView.GetRootSwitchableOwner(out var applicationViewPtr);
        return new ApplicationView((IApplicationView)Marshal.GetObjectForIUnknown(applicationViewPtr));
    }

    public ObjectArray<T> EnumerateOwnershipTree<T>() where T : class
    {
        m_ApplicationView.EnumerateOwnershipTree(out var objectArrayPtr);
        return new ObjectArray<T>((IObjectArray)Marshal.GetObjectForIUnknown(objectArrayPtr));
    }

    public string GetEnterpriseId()
    {
        m_ApplicationView.GetEnterpriseId(out var enterpriseId);
        return enterpriseId;
    }

    public bool IsMirrored()
    {
        m_ApplicationView.IsMirrored(out var isMirrored);
        return isMirrored;
    }

    public uint GetFrameworkViewType()
    {
        m_ApplicationView.GetFrameworkViewType(out var viewTypeId);
        return viewTypeId;
    }

    public uint GetCanTab()
    {
        m_ApplicationView.GetCanTab(out var canTab);
        return canTab;
    }

    public void SetCanTab(uint canTab)
    {
        m_ApplicationView.SetCanTab(canTab);
    }

    public uint GetIsTabbed()
    {
        m_ApplicationView.GetIsTabbed(out var isTabbed);
        return isTabbed;
    }

    public void SetIsTabbed(uint isTabbed)
    {
        m_ApplicationView.SetIsTabbed(isTabbed);
    }

    public void RefreshCanTab()
    {
        m_ApplicationView.RefreshCanTab();
    }

    public uint GetIsOccluded()
    {
        m_ApplicationView.GetIsOccluded(out var isOccluded);
        return isOccluded;
    }

    public void SetIsOccluded(uint isOccluded)
    {
        m_ApplicationView.SetIsOccluded(isOccluded);
    }

    // Assuming previous methods are defined above this point...

    public void UpdateEngagementFlags(uint flags, uint value)
    {
        m_ApplicationView.UpdateEngagementFlags(flags, value);
    }

    public void SetForceActiveWindowAppearance(uint forceActive)
    {
        m_ApplicationView.SetForceActiveWindowAppearance(forceActive);
    }

    public SIZE GetLastActivationFILETIME()
    {
        m_ApplicationView.GetLastActivationFILETIME(out var fileTime);
        return fileTime;
    }

    public string GetPersistingStateName()
    {
        m_ApplicationView.GetPersistingStateName(out var stateName);
        return stateName;
    }
}