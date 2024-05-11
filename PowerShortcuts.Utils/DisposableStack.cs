namespace PowerShortcuts.Utils;

public class DisposableStack: IDisposable
{
    private readonly Stack<IDisposable> m_Stack = new();

    public void PushRange(IEnumerable<IDisposable> enumerable)
    {
        foreach (var item in enumerable)
        {
            m_Stack.Push(item);
        }
    }

    public void Push(IDisposable disposable)
    {
        m_Stack.Push(disposable);
    }

    public void DisposeItems()
    {
        foreach (var element in m_Stack)
        {
            element.Dispose();
        }
        m_Stack.Clear();
    }

    public void Dispose()
    {
        DisposeItems();
        GC.SuppressFinalize(this);
    }
}