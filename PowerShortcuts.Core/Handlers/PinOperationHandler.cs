using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using Microsoft.Extensions.Logging;
using PowerShortcuts.Core.Interface;
using PowerShortcuts.VirtualDesktop.Interface;

namespace PowerShortcuts.Core.Handlers;

internal sealed class PinOperationHandler(
    ILogger<PinOperationHandler> logger,
    IWindowManager windowManager,
    IVirtualDesktopManager desktopManager) : IPowerShortcutHandler
{
    private const VirtualKeyCode PinKey = VirtualKeyCode.KEY_A;
    private const Modifiers PinModifier = Modifiers.Shift | Modifiers.Control;
    
    public Guid Id => DefaultHandlersGuids.PinOperation;

    public bool HandleShortcut(HotKey hotKey)
    {
        if (hotKey is not { Key: PinKey, Modifiers: PinModifier }) return false;

        TogglePin();
        return true;
    }

    public void RegisterHotkeys(IHotKeyRegistration registration)
    {
        registration.RegisterHotKey(PinKey, PinModifier);
    }

    private void TogglePin()
    {
        var windowInFocusHwnd = windowManager.GetWindowInFocus();
        var hasFocusedWindow = windowInFocusHwnd != IntPtr.Zero;
        var focusedWindowTitle = hasFocusedWindow ? windowManager.GetWindowTitle(windowInFocusHwnd) : null;

        if (!hasFocusedWindow || focusedWindowTitle is null)
        {
            PinOperationHandlerLogging.LogNoPinWindowInFocus(logger);
            return;
        }

        var isPinnedApp = desktopManager.IsPinnedApp(windowInFocusHwnd);

        // We don't pin apps, sounds unusable
        if (isPinnedApp)
        {
            desktopManager.UnPinApp(windowInFocusHwnd);
            PinOperationHandlerLogging.LogUnpinApp(logger, focusedWindowTitle);
            return;
        }

        var isPinnedWindow = desktopManager.IsPinnedWindow(windowInFocusHwnd);

        if (isPinnedWindow)
        {
            desktopManager.UnPinWindow(windowInFocusHwnd);
            PinOperationHandlerLogging.LogUnpinWindow(logger, focusedWindowTitle);
        }
        else
        {
            desktopManager.PinWindow(windowInFocusHwnd);
            PinOperationHandlerLogging.LogPinWindow(logger, focusedWindowTitle);
        }
    }
}