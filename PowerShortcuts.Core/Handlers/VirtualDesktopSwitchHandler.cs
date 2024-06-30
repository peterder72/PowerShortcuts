using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using Microsoft.Extensions.Logging;
using PowerShortcuts.Core.Interface;
using PowerShortcuts.VirtualDesktop.Interface;

namespace PowerShortcuts.Core.Handlers;

internal sealed class VirtualDesktopSwitchHandler(ILogger<VirtualDesktopSwitchHandler> logger, IVirtualDesktopManager desktopManager, IWindowManager windowManager) : IPowerShortcutHandler
{
    private const Modifiers SwitchDesktopModifier = Modifiers.Alt;
    private const Modifiers MoveWindowModifier = Modifiers.Alt | Modifiers.Shift;

    private static readonly Dictionary<VirtualKeyCode, int> NumberKeyCodes = new()
    {
        { VirtualKeyCode.KEY_1, 0 },
        { VirtualKeyCode.KEY_2, 1 },
        { VirtualKeyCode.KEY_3, 2 },
        { VirtualKeyCode.KEY_4, 3 },
        { VirtualKeyCode.KEY_5, 4 },
        { VirtualKeyCode.KEY_6, 5 },
        { VirtualKeyCode.KEY_7, 6 },
        { VirtualKeyCode.KEY_8, 7 },
        { VirtualKeyCode.KEY_9, 8 },
        { VirtualKeyCode.KEY_0, 9 }
    };

    public Guid Id => DefaultHandlersGuids.VirtualDesktopSwitch;

    public bool HandleShortcut(HotKey hotKey)
    {
        var isNumberKey = NumberKeyCodes.TryGetValue(hotKey.Key, out var desktopNumber);

        if (!isNumberKey || !IsValidDesktopNumber(desktopNumber))
        {
            VirtualDesktopSwitchLogging.CantHandleShortcut(logger, hotKey.Key, hotKey.Modifiers);
            return false;
        }
        
        if (hotKey.Modifiers == SwitchDesktopModifier)
        {
            SwitchToDesktop(desktopNumber);
        }
        else if (hotKey.Modifiers == MoveWindowModifier)
        {
            MoveWindowToDesktop(desktopNumber);
        }
        else
        {
            VirtualDesktopSwitchLogging.UnknownModifier(logger, hotKey.Modifiers);
            return false;
        }

        return true;
    }

    public void RegisterHotkeys(IHotKeyRegistration registration)
    {
        foreach (var key in NumberKeyCodes.Keys)
        {
            registration.RegisterHotKey(key, SwitchDesktopModifier);
            registration.RegisterHotKey(key, MoveWindowModifier);
        }
    }

    private bool IsValidDesktopNumber(int desktopNumber)
    {
        var numDesktops = desktopManager.GetDesktopCount();
        return desktopNumber < numDesktops;
    }
    
    
    private void MoveWindowToDesktop(int desktopNumber)
    {
        var windowInFocusHwnd = windowManager.GetWindowInFocus();
        if (windowInFocusHwnd == IntPtr.Zero)
        {
           VirtualDesktopSwitchLogging.NoWindowInFocus(logger);
           return;
        };
        
        var windowName = windowManager.GetWindowTitle(windowInFocusHwnd);

        var currentWindowDesktop = desktopManager.GetWindowDesktopNumber(windowInFocusHwnd);
        if (currentWindowDesktop == desktopNumber)
        {
            VirtualDesktopSwitchLogging.WindowAlreadyOnDesktop(logger, windowName, desktopNumber, currentWindowDesktop);
            return;
        }
        
        var success = desktopManager.MoveWindowToDesktopNumber(windowInFocusHwnd, desktopNumber);
        if (!success)
        {
            VirtualDesktopSwitchLogging.WindowMoveUnsuccessful(logger, windowName, desktopNumber);
        }
        else
        {
           VirtualDesktopSwitchLogging.WindowMoveSuccessful(logger, windowName, desktopNumber); 
        }
    }

    private void SwitchToDesktop(int desktopNumber)
    {
        // var currentDesktop = desktopManager
        var success = desktopManager.GoToDesktopNumber(desktopNumber);

        if (success)
        {
            VirtualDesktopSwitchLogging.DesktopSwitchSuccessful(logger, desktopNumber);
        } else
        {
            VirtualDesktopSwitchLogging.DesktopSwitchUnsuccessful(logger, desktopNumber);
        }
    }
}