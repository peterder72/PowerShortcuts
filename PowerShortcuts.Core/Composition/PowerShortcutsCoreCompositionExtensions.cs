using Microsoft.Extensions.DependencyInjection;
using PowerShortcuts.Core.Handlers;
using PowerShortcuts.Core.Interface;
using PowerShortcuts.VirtualDesktop.Composition;

namespace PowerShortcuts.Core.Composition;

public static class PowerShortcutsCoreCompositionExtensions
{
    public static IServiceCollection AddPowerShortcutsCore(this IServiceCollection services)
    {
        services.AddVirtualDesktop();
        services.AddDefaultHandlers();
        
        services.AddSingleton<IPowerShortcutsService, PowerShortcutsService>();
        services.AddScoped<IHandlerLifetimeFactory, HandlerLifetimeFactory>();
        services.AddScoped<IHandlerOrderProvider, HardcodedDefaultHandlerOrderProvider>();

        return services;
    }
    
    private static IServiceCollection AddDefaultHandlers(this IServiceCollection services)
    {
        services.AddScoped<IPowerShortcutHandler, PinOperationHandler>();
        services.AddScoped<IPowerShortcutHandler, VirtualDesktopSwitchHandler>();

        return services;
    }
}