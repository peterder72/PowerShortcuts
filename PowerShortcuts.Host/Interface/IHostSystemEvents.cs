namespace PowerShortcuts.Host.Interface;

internal interface IHostSystemEvents
{
    void SystemExitRequested();
    IObservable<bool> ExitRequestedObservable { get; }
}