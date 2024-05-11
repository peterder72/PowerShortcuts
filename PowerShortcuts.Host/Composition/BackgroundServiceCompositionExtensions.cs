using PowerShortcuts.Host.Interface;

namespace PowerShortcuts.Host.Composition;

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