using System.Runtime.InteropServices;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
internal interface IObjectArray
{
    int GetCount(out uint pcObjects);

    void GetAt(uint i, ref Guid riid, out IntPtr ppvObject);
}
