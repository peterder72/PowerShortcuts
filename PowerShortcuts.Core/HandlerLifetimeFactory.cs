using Microsoft.Extensions.DependencyInjection;
using PowerShortcuts.Core.Interface;

namespace PowerShortcuts.Core;

internal sealed class HandlerLifetimeFactory(IServiceScopeFactory serviceScopeFactory, IHandlerOrderProvider orderProvider) : IHandlerLifetimeFactory
{
    public IHandlerLifetime Create()
    {
        var scope = serviceScopeFactory.CreateScope();
        
        var handlers = scope.ServiceProvider.GetServices<IPowerShortcutHandler>()
            .Select(x => (Order: orderProvider.GetOrder(x), Handler: x))
            .OrderBy(x => x.Order)
            .Select(x => x.Handler)
            .ToArray();
        
        return new HandlerLifetime(handlers, () => scope.Dispose());
    }

    private sealed class HandlerLifetime(IPowerShortcutHandler[] handlers, Action disposeAction) : IHandlerLifetime
    {
        public IPowerShortcutHandler[] Handlers { get; } = handlers;
        public void Dispose() => disposeAction();
    }

}