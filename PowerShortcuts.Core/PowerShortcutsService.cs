using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using Microsoft.Extensions.Logging;
using PowerShortcuts.Core.Interface;
using PowerShortcuts.Utils;
using PowerShortcuts.VirtualDesktop.Interface;

namespace PowerShortcuts.Core;

internal sealed class PowerShortcutsService(
    IVirtualDesktopManager desktopManager,
    IWindowManager windowManager,
    ILogger<IPowerShortcutsService> logger) : IPowerShortcutsService
{
    private const Modifiers SwitchDesktopModifier = Modifiers.Alt;
    private const Modifiers MoveWindowModifier = Modifiers.Alt | Modifiers.Shift;

    private readonly DisposableStack m_SubscriptionDisposables = new();
    private bool m_Initialized;
    private bool m_Disposed;

    private readonly VirtualKeyCode[] m_NumberKeyCodes =
    [
        VirtualKeyCode.KEY_1,
        VirtualKeyCode.KEY_2,
        VirtualKeyCode.KEY_3,
        VirtualKeyCode.KEY_4,
        VirtualKeyCode.KEY_5,
        VirtualKeyCode.KEY_6,
        VirtualKeyCode.KEY_7,
        VirtualKeyCode.KEY_8,
        VirtualKeyCode.KEY_9,
        VirtualKeyCode.KEY_0
    ];

    public void Initialize()
    {
        if (m_Initialized) throw new InvalidOperationException("PowerShortcutsService already initialized");

        var hotKeyManager = new HotKeyManager();
        var desktopSubscription = hotKeyManager.HotKeyPressed.Subscribe(DesktopHotKeyPressed);
        var registrations = new DisposableStack();

        m_SubscriptionDisposables.Push(desktopSubscription);
        m_SubscriptionDisposables.Push(hotKeyManager);
        m_SubscriptionDisposables.Push(registrations);

        var switchDesktopRegistrations = m_NumberKeyCodes
            .Select(key => hotKeyManager.Register(key, SwitchDesktopModifier))
            .ToArray();

        var moveWindowRegistrations = m_NumberKeyCodes
            .Select(key => hotKeyManager.Register(key, MoveWindowModifier))
            .ToArray();

        var pinWindowRegistration = hotKeyManager.Register(VirtualKeyCode.KEY_A, Modifiers.Shift | Modifiers.Control);
        registrations.Push(pinWindowRegistration);

        registrations.PushRange(switchDesktopRegistrations);
        registrations.PushRange(moveWindowRegistrations);

        m_Initialized = true;

        logger.LogInformation("Shortcuts service initialized");
    }

    public void Terminate()
    {
        m_SubscriptionDisposables.DisposeItems();
        m_Initialized = false;
        logger.LogInformation("Shortcuts service terminated");
    }

    private void DesktopHotKeyPressed(HotKey hotKey)
    {
        var windowInFocus = windowManager.GetWindowInFocus();
        var focusedWindowTitle = windowInFocus != IntPtr.Zero ? windowManager.GetWindowTitle(windowInFocus) : null;

        if (hotKey is { Key: VirtualKeyCode.KEY_A, Modifiers: (Modifiers.Shift | Modifiers.Control) })
        {
            if (windowInFocus == IntPtr.Zero) return;

            var isPinnedApp = desktopManager.IsPinnedApp(windowInFocus);

            // We don't pin apps, sounds unusable
            if (isPinnedApp)
            {
                desktopManager.UnPinApp(windowInFocus);
                return;
            }

            var isPinnedWindow = desktopManager.IsPinnedWindow(windowInFocus);

            if (isPinnedWindow)
            {
                desktopManager.UnPinWindow(windowInFocus);
            }
            else
            {
                desktopManager.PinWindow(windowInFocus);
            }

            return;
        }

        var desktopNumber = hotKey.Key switch
        {
            VirtualKeyCode.KEY_1 => 0,
            VirtualKeyCode.KEY_2 => 1,
            VirtualKeyCode.KEY_3 => 2,
            VirtualKeyCode.KEY_4 => 3,
            VirtualKeyCode.KEY_5 => 4,
            VirtualKeyCode.KEY_6 => 5,
            VirtualKeyCode.KEY_7 => 6,
            VirtualKeyCode.KEY_8 => 7,
            VirtualKeyCode.KEY_9 => 8,
            VirtualKeyCode.KEY_0 => 9,
            _ => throw new ArgumentException()
        };

        var numDesktops = desktopManager.GetDesktopCount();

        if (desktopNumber > numDesktops - 1) return;

        switch (hotKey.Modifiers)
        {
            case SwitchDesktopModifier:
                desktopManager.GoToDesktopNumber(desktopNumber);
                break;
            case MoveWindowModifier:
            {
                var currentWindowDesktop = desktopManager.GetWindowDesktopNumber(windowInFocus);

                if (currentWindowDesktop == desktopNumber) return;

                desktopManager.MoveWindowToDesktopNumber(windowInFocus, desktopNumber);
                break;
            }
        }
    }

    public void Dispose()
    {
        if (m_Disposed) throw new ObjectDisposedException(nameof(PowerShortcutsService));
        m_Initialized = false;
        m_SubscriptionDisposables.Dispose();

        logger.LogInformation("Shortcuts service disposed");
        m_Disposed = true;
    }
}