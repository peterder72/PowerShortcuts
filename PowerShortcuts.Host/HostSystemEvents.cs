using System.Reactive.Subjects;
using PowerShortcuts.WinService.Interface;

namespace PowerShortcuts.WinService;

internal sealed class HostSystemEvents(ILogger<HostSystemEvents> logger): 
    IHostSystemEvents,
    IDisposable
{
    private bool m_Disposed;
    private readonly Subject<bool> m_ExitRequestedSubject = new();
    
    public void SystemExitRequested()
    {
        logger.LogInformation("System exit requested");
        m_ExitRequestedSubject.OnNext(true);
    }

    public IObservable<bool> ExitRequestedObservable => m_ExitRequestedSubject;

    public void Dispose()
    {
        if (m_Disposed) throw new ObjectDisposedException(nameof(HostSystemEvents));
        
        m_ExitRequestedSubject.Dispose();
        m_Disposed = true;
    }
}