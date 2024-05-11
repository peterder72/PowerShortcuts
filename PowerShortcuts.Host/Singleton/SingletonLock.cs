namespace PowerShortcuts.Host.Singleton;

internal sealed class SingletonLock: IDisposable
{
    private readonly Mutex m_Mutex;

    public SingletonLock(string name)
    {
        m_Mutex = new Mutex(true, name, out var createdNew);
        
        AnotherInstanceRunning = !createdNew;
    }

    public void Dispose()
    {
        m_Mutex.Dispose();
    }
    
    public bool AnotherInstanceRunning { get; }
}