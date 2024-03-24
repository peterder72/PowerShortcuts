using PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Implementation;

internal class ApplicationViewCollection
{
    private readonly IApplicationViewCollection m_ApplicationViewCollection;

    public ApplicationViewCollection(IApplicationViewCollection applicationViewCollection)
    {
        m_ApplicationViewCollection = applicationViewCollection ?? throw new ArgumentNullException(nameof(applicationViewCollection));
    }

    public ObjectArray<IApplicationView> GetViews()
    {
        m_ApplicationViewCollection.GetViews(out var objectArray);
        return new ObjectArray<IApplicationView>(objectArray);
    }

    public ObjectArray<IApplicationView> GetViewsByZOrder()
    {
        m_ApplicationViewCollection.GetViewsByZOrder(out var objectArray);
        return new ObjectArray<IApplicationView>(objectArray);
    }

    public ObjectArray<IApplicationView> GetViewsByAppUserModelId(string appUserModelId)
    {
        m_ApplicationViewCollection.GetViewsByAppUserModelId(appUserModelId, out var objectArray);
        return new ObjectArray<IApplicationView>(objectArray);
    }

    public ApplicationView GetViewForHwnd(IntPtr hwnd)
    {
        m_ApplicationViewCollection.GetViewForHwnd(hwnd, out var view);
        return new ApplicationView(view);
    }

    public ApplicationView GetViewForApplication(uint immersiveApplication)
    {
        m_ApplicationViewCollection.GetViewForApplication(immersiveApplication, out var view);
        return new ApplicationView(view);
    }

    public ApplicationView GetViewForAppUserModelId(string appUserModelId)
    {
        m_ApplicationViewCollection.GetViewForAppUserModelId(appUserModelId, out var view);
        return new ApplicationView(view);
    }

    public ApplicationView GetViewInFocus()
    {
        m_ApplicationViewCollection.GetViewInFocus(out var view);
        return new ApplicationView(view);
    }

    public void RefreshCollection()
    {
        m_ApplicationViewCollection.RefreshCollection();
    }

    // public uint RegisterForApplicationViewChanges(IApplicationViewChangeListener listener)
    // {
    //     m_ApplicationViewCollection.RegisterForApplicationViewChanges(listener, out var cookie);
    //     return cookie;
    // }
    //
    // public void UnregisterForApplicationViewChanges(uint cookie)
    // {
    //     m_ApplicationViewCollection.UnregisterForApplicationViewChanges(cookie);
    // }
}
