using System.Drawing;
using System.Reactive.Subjects;
using H.NotifyIcon.Core;
using PowerShortcuts.Host.Interface;
using PowerShortcuts.Utils;

namespace PowerShortcuts.Host;

internal sealed class PowerShortcutsTrayMenu : IPowerShortcutsTrayControl
{
    private readonly ILogger<IPowerShortcutsTrayControl> m_Logger;
    private readonly DisposableStack m_Disposables = new();
    private readonly TrayIconWithContextMenu m_TrayIcon;
    private readonly Subject<TrayClickEventType> m_TrayClicksSubject = new();
    private bool m_Disposed;

    public PowerShortcutsTrayMenu(ILogger<IPowerShortcutsTrayControl> logger)
    {
        m_Logger = logger;
        var iconStream = H.Resources.lightning_ico.AsStream();
        m_Disposables.Push(iconStream);
        
        var icon = new Icon(iconStream);
        m_Disposables.Push(icon);
        
        var trayIcon = new TrayIconWithContextMenu
        {
            Icon = icon.Handle,
            ToolTip = "PowerShortcuts",
        };
        m_Disposables.Push(trayIcon);

        trayIcon.ContextMenu = new PopupMenu
        {
            Items =
            {
                new PopupMenuItem("Exit", (_, _) => m_TrayClicksSubject.OnNext(TrayClickEventType.Exit)),
            },
        };

        m_TrayIcon = trayIcon;
        
    }

    public void Create()
    {
        if (m_TrayIcon.IsCreated) throw new InvalidOperationException("Tray icon already created");
        
        m_TrayIcon.Create();
        
        m_Logger.LogInformation("Tray menu created");
    }

    public void Terminate()
    {
        if (!m_TrayIcon.IsCreated) throw new InvalidOperationException("Tray icon not created");
        
        m_TrayIcon.ClearNotifications();
        m_TrayIcon.Remove();
        
        m_Logger.LogInformation("Tray menu terminated");
    }

    public void ShowNotification(string title, string message, NotificationIconType iconType)
    {
        var icon = iconType switch
        {
            NotificationIconType.Info => NotificationIcon.Info,
            NotificationIconType.Warning => NotificationIcon.Warning,
            NotificationIconType.Error => NotificationIcon.Error,
            NotificationIconType.None => NotificationIcon.None,
            _ => throw new ArgumentOutOfRangeException(nameof(iconType), iconType, null)
        };
        
        m_TrayIcon.ShowNotification(title, message, icon);
    }

    public IObservable<TrayClickEventType> TrayClicksObservable => m_TrayClicksSubject;

    public void Dispose()
    {
        if (m_Disposed) throw new ObjectDisposedException(nameof(PowerShortcutsTrayMenu));
        
        if (m_TrayIcon.IsCreated)
        {
            m_TrayIcon.ClearNotifications();
            m_TrayIcon.Remove();
        }
        
        m_TrayClicksSubject.Dispose();
        m_Disposables.Dispose();
        m_Logger.LogInformation("Tray icon disposed");
        
        m_Disposed = true;
    }
}