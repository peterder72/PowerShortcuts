using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using PowerShortcuts.Core.Interface;
using PowerShortcuts.VirtualDesktop.Composition;

namespace PowerShortcuts.Core.Composition;

public static class PowerShortcutsCoreCompositionExtensions
{
    public static IServiceCollection AddPowerShortcutsCore(this IServiceCollection services)
    {
        services.AddVirtualDesktop();
        services.AddSingleton<IPowerShortcutsService, PowerShortcutsService>();

        return services;
    }
}