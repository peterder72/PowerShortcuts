using System.Runtime.InteropServices;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
internal interface IServiceProvider
{
    [PreserveSig]
    int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject);
}
