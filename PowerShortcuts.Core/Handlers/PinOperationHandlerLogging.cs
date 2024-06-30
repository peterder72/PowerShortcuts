using Microsoft.Extensions.Logging;

namespace PowerShortcuts.Core.Handlers;

internal partial class PinOperationHandlerLogging
{
    [LoggerMessage(LogLevel.Debug, "No window in focus, cannot pin/unpin")]
    public static partial void LogNoPinWindowInFocus(ILogger logger);

    [LoggerMessage(LogLevel.Debug, "Unpinning app: {windowTitle}")]
    public static partial void LogUnpinApp(ILogger logger, string windowTitle);

    [LoggerMessage(LogLevel.Debug, "Pinning window: {windowTitle}")]
    public static partial void LogPinWindow(ILogger logger, string windowTitle);

    [LoggerMessage(LogLevel.Debug, "Unpinning window: {windowTitle}")]
    public static partial void LogUnpinWindow(ILogger logger, string windowTitle);
}