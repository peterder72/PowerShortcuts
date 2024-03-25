using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using PowerShortcuts.VirtualDesktop.Interface;

namespace PowerShortcuts.VirtualDesktop.Composition;

public static class VirtualDesktopCompositionExtensions
{
    public static IServiceCollection AddVirtualDesktop(this IServiceCollection services)
    {
        services.AddSingleton<IWindowManager, WindowManager>();
        services.AddSingleton<IVirtualDesktopManager, VirtualDesktopManager>();

        return services;
    }
    
}