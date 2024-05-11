namespace PowerShortcuts.WinService.Interface;

internal interface IHostSystemEvents
{
    void SystemExitRequested();
    IObservable<bool> ExitRequestedObservable { get; }
}