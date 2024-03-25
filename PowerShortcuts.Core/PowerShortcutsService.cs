using System.Net.Http.Headers;
using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using PowerShortcuts.Core.Interface;
using PowerShortcuts.Host;
using PowerShortcuts.VirtualDesktop.Interface;

namespace PowerShortcuts.Core;

internal sealed class PowerShortcutsService(IVirtualDesktopManager desktopManager, IWindowManager windowManager): IPowerShortcutsService
{
    private const Modifiers SwitchDesktopModifier = Modifiers.Alt;
    private const Modifiers MoveWindowModifier = Modifiers.Alt | Modifiers.Shift;

    private readonly DisposableStack m_Disposables = new();
    private bool m_Initialized = false;

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
        
        m_Disposables.Push(desktopSubscription);
        m_Disposables.Push(hotKeyManager);
        m_Disposables.Push(registrations);
            
        var switchDesktopRegistrations = m_NumberKeyCodes
            .Select(key => hotKeyManager.Register(key, SwitchDesktopModifier))
            .ToArray();

        var moveWindowRegistrations = m_NumberKeyCodes
            .Select(key => hotKeyManager.Register(key, MoveWindowModifier))
            .ToArray();

        registrations.PushRange(switchDesktopRegistrations);
        registrations.PushRange(moveWindowRegistrations);

        m_Initialized = true;
    }
    
    
    private void DesktopHotKeyPressed(HotKey hotKey)
    {
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
                var windowInFocus = windowManager.GetWindowInFocus();
                var currentWindowDesktop = desktopManager.GetWindowDesktopNumber(windowInFocus);

                if (currentWindowDesktop == desktopNumber) return;

                desktopManager.MoveWindowToDesktopNumber(windowInFocus, desktopNumber);
                break;
            }
        }
    }

    public void Dispose()
    {
        Console.WriteLine("Destroyed");
        m_Disposables.Dispose();
        m_Initialized = false;
    }
}