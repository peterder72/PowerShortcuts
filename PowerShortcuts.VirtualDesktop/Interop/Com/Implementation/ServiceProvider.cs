using System.Runtime.InteropServices;
using IServiceProvider = PowerShortcuts.VirtualDesktop.Interop.Com.Interface.IServiceProvider;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Implementation;

internal class ServiceProvider: IDisposable
{
    private readonly UnmanagedDisposableContainer<IServiceProvider> m_ServiceProvider;
    private static readonly Guid ImmersiveShellClsId = new("c2f03a33-21f5-47fa-b4bb-156362a2f239");

    private ServiceProvider(UnmanagedDisposableContainer<IServiceProvider> serviceProvider)
    {
        m_ServiceProvider = serviceProvider;
    }

    public static ServiceProvider Create()
    {
        var shellType = Type.GetTypeFromCLSID(ImmersiveShellClsId) ?? 
                        throw new Exception("Could not fetch ImmersiveShell");
        
        var shell = Activator.CreateInstance(shellType);
        
        var comServiceProvider = new UnmanagedDisposableContainer<IServiceProvider>(shell);

        return new ServiceProvider(comServiceProvider);
    }
    
    
    public UnmanagedDisposableContainer<T> QueryService<T>(Guid serviceGuid) where T : class
    {
        var interfaceGuid = typeof(T).GUID;
        m_ServiceProvider.Value.QueryService(ref serviceGuid, ref interfaceGuid, out IntPtr servicePtr);

        if (servicePtr == IntPtr.Zero)
        {
            throw new Exception($"Could not cast {serviceGuid} to {typeof(T)} ({interfaceGuid}) ");
        }

        try
        {
            return new UnmanagedDisposableContainer<T>(Marshal.GetObjectForIUnknown(servicePtr));
        }
        finally
        {
            Marshal.Release(servicePtr);
        }
    }


    public void Dispose()
    {
        m_ServiceProvider.Dispose();
    }
}