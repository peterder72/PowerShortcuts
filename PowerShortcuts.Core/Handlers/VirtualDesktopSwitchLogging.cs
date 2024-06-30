using GlobalHotKeys.Native.Types;
using Microsoft.Extensions.Logging;

namespace PowerShortcuts.Core.Handlers;

internal static partial class VirtualDesktopSwitchLogging
{
    [LoggerMessage(LogLevel.Debug, "Cannot handle virtual desktop switch, unknown modifier: {modifiers}")]
    public static partial void UnknownModifier(ILogger logger, Modifiers modifiers);
    
    [LoggerMessage(LogLevel.Debug, "Cannot handle virtual desktop switch, unknown shortcut: {key} + {modifiers}")]
    public static partial void CantHandleShortcut(ILogger logger, VirtualKeyCode key, Modifiers modifiers);
    
    [LoggerMessage(LogLevel.Debug, "No window in focus, cannot move anything to any desktop")]
    public static partial void NoWindowInFocus(ILogger logger);
    
    [LoggerMessage(LogLevel.Debug, "Window {windowName} is already on desktop {currentDesktop} (requested {requestedDesktop}), not moving")]
    public static partial void WindowAlreadyOnDesktop(ILogger logger, string windowName, int requestedDesktop, int currentDesktop);
    
    [LoggerMessage(LogLevel.Debug, "Already on desktop {currentDesktop} (requested {requestedDesktop}), not switching")]
    public static partial void AlreadyOnDesktop(ILogger logger, int requestedDesktop, int currentDesktop);
    
    [LoggerMessage(LogLevel.Debug, "Moved window {windowName} to desktop {requestedDesktop}")]
    public static partial void WindowMoveSuccessful(ILogger logger, string windowName, int requestedDesktop);
    
    [LoggerMessage(LogLevel.Error, "Could not move window {windowName} to desktop {requestedDesktop}")]
    public static partial void WindowMoveUnsuccessful(ILogger logger, string windowName, int requestedDesktop);
    
    [LoggerMessage(LogLevel.Debug, "Switched to desktop {requestedDesktop}")]
    public static partial void DesktopSwitchSuccessful(ILogger logger, int requestedDesktop);
    
    [LoggerMessage(LogLevel.Error, "Could not switch to desktop {requestedDesktop}")]
    public static partial void DesktopSwitchUnsuccessful(ILogger logger, int requestedDesktop);

}