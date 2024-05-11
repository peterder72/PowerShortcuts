using PowerShortcuts.Core.Interface;
using PowerShortcuts.Host.Interface;

namespace PowerShortcuts.Host;

internal sealed class WindowsBackgroundService(
    IPowerShortcutsService powerShortcutsService,
    IPowerShortcutsTrayControl powerShortcutsTrayControl,
    IHostSystemEvents hostSystemEvents,
    ILogger<WindowsBackgroundService> logger) : IHostedService 
{
    private const string NotificationTitle = "PowerShortcuts";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("PowerShortcutsService starting");

        try
        {
            powerShortcutsTrayControl.TrayClicksObservable.Subscribe(OnTrayClick);
            powerShortcutsTrayControl.Create();
            powerShortcutsService.Initialize();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Init error: {Message}", ex.Message);

            powerShortcutsTrayControl.ShowNotification(NotificationTitle,
                $"Error during initialization: {ex.Message}",
                NotificationIconType.Error);

            throw;
        }

        powerShortcutsTrayControl.ShowNotification(NotificationTitle, "PowerShortcuts is running",
            NotificationIconType.Info);

        return Task.CompletedTask;
    }

    private void OnTrayClick(TrayClickEventType ev)
    {
        switch (ev)
        {
            case TrayClickEventType.Exit:
                hostSystemEvents.SystemExitRequested();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ev), ev, null);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("PowerShortcutsService shutting down");

        powerShortcutsService.Terminate();
        powerShortcutsTrayControl.Terminate();

        return Task.CompletedTask;
    }

}