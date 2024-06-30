using System.Diagnostics.CodeAnalysis;
using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using Microsoft.Extensions.Logging;
using PowerShortcuts.Core.Interface;
using PowerShortcuts.Utils;
using PowerShortcuts.VirtualDesktop.Interface;

namespace PowerShortcuts.Core;

internal sealed class PowerShortcutsService(ILogger<IPowerShortcutsService> logger, IHandlerLifetimeFactory handlerLifetimeFactory) : IPowerShortcutsService
{
    private readonly DisposableStack m_SubscriptionDisposables = new();
    private bool m_Initialized;
    private bool m_Disposed;

    public void Initialize()
    {
        if (m_Initialized) throw new InvalidOperationException("PowerShortcutsService already initialized");

        var hotKeyManager = new HotKeyManager();
        var handlerLifetime = handlerLifetimeFactory.Create();
        
        var registration = new ServiceHotKeyRegistration(hotKeyManager);
        foreach (var handler in handlerLifetime.Handlers)
        {
            handler.RegisterHotkeys(registration);
        }
        
        var hotKeyHandler = new HotKeyHandler(handlerLifetime.Handlers);
        
        var desktopSubscription = hotKeyManager.HotKeyPressed.Subscribe(hotKeyHandler);

        m_SubscriptionDisposables.Push(desktopSubscription);
        m_SubscriptionDisposables.Push(hotKeyManager);
        m_SubscriptionDisposables.Push(registration.RegistrationDisposables);

        m_Initialized = true;

        logger.LogInformation("Shortcuts service initialized");
    }

    public void Terminate()
    {
        m_SubscriptionDisposables.DisposeItems();
        m_Initialized = false;
        logger.LogInformation("Shortcuts service terminated");
    }

    public void Dispose()
    {
        if (m_Disposed) throw new ObjectDisposedException(nameof(PowerShortcutsService));
        m_Initialized = false;
        m_SubscriptionDisposables.Dispose();

        logger.LogInformation("Shortcuts service disposed");
        m_Disposed = true;
    }

    private sealed class ServiceHotKeyRegistration(HotKeyManager hotKeyManager) : IHotKeyRegistration
    {
        public DisposableStack RegistrationDisposables { get; } = new();
        
        public void RegisterHotKey(VirtualKeyCode key, Modifiers modifiers)
        {
            var registration = hotKeyManager.Register(key, modifiers);
            RegistrationDisposables.Push(registration);
        }
    }
    
    private sealed class HotKeyHandler(IPowerShortcutHandler[] handlers): IObserver<HotKey>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(HotKey hotKey)
        {
            foreach (var handler in handlers)
            {
                var handled = handler.HandleShortcut(hotKey);
                
                if (handled) break;
            }
        }
    }
}