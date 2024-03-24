using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using PowerShortcuts.VirtualDesktop.Interop.Com.Interface;

namespace PowerShortcuts.VirtualDesktop.Interop.Com.Implementation;

internal class ObjectArray<T> : IEnumerable<T> where T : class
{
    private readonly IObjectArray m_ObjectArray;

    public ObjectArray(IObjectArray objectArray)
    {
        m_ObjectArray = objectArray ?? throw new ArgumentNullException(nameof(objectArray));
    }

    public int Count
    {
        get
        {
            m_ObjectArray.GetCount(out var count);
            return (int)count;
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            var iid = typeof(T).GUID;
            m_ObjectArray.GetAt((uint)index, ref iid, out var ptr);

            if (ptr == IntPtr.Zero) ThrowNullException();

            return (T)Marshal.GetObjectForIUnknown(ptr);
        }
    }

    [DoesNotReturn]
    private static T ThrowNullException()
    {
        throw new NullReferenceException("Null found in the object array");
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
