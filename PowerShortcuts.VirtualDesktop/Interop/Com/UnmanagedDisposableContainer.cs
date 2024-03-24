using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PowerShortcuts.VirtualDesktop.Interop.Com;

internal sealed class UnmanagedDisposableContainer<T>(object? obj) : IDisposable
    where T : class
{
    private T? m_Value = obj as T ?? throw new Exception($"Could not cast to {typeof(T)}");

    public T Value => m_Value ?? ThrowDisposedException();

    public void Dispose()
    {
        Marshal.ReleaseComObject(Value);
        m_Value = null;
        
        GC.SuppressFinalize(this);
    }

    ~UnmanagedDisposableContainer()
    {
        Dispose();
    }

    [DoesNotReturn]
    private static T ThrowDisposedException() => 
        throw new ObjectDisposedException(typeof(T).FullName);
}