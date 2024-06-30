using Microsoft.Extensions.Logging;

namespace PowerShortcuts.VirtualDesktop;

internal static partial class WindowManagerLogging
{
    [LoggerMessage(LogLevel.Warning, "Could not get window title, return code={returnCode}")]
    public static partial void LogUnsuccessfulGetWindowTitle(ILogger logger, int returnCode);
}