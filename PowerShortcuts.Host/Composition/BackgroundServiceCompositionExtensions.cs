using PowerShortcuts.WinService.Interface;

namespace PowerShortcuts.WinService.Composition;

internal static class BackgroundServiceCompositionExtensions
{
    
  public static void AddPowerShortcutsTray(this IServiceCollection services)
    {
        services.AddSingleton<IPowerShortcutsTrayControl, PowerShortcutsTrayMenu>();
    }

    public static void AddBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<WindowsBackgroundService>();
        services.AddSingleton<IHostSystemEvents, HostSystemEvents>();
    }
}