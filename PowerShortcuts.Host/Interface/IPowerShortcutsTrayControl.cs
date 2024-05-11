namespace PowerShortcuts.WinService.Interface;

internal interface IPowerShortcutsTrayControl: IDisposable
{
    void Create();
    void Terminate();
    void ShowNotification(string title, string message, NotificationIconType iconType);
    public IObservable<TrayClickEventType> TrayClicksObservable { get; }
}