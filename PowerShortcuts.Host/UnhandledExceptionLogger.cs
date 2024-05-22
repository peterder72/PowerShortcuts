using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
// using OSVersionExtension;

namespace PowerShortcuts.Host;

internal static class UnhandledExceptionLogger
{
    public static void Initialize()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            var exception = (Exception)args.ExceptionObject;
            LogException(exception);
            DisplayExceptionMessageBox(exception);
        };
    }
    
    static void LogException(Exception exception)
    {
        var fileName = $"PowerShortcutsCrash_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";
        
        var crashLogsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Osetr", "CrashLogs");
        Directory.CreateDirectory(crashLogsDirectory);
        
        var sb = new StringBuilder();
        
        AddWindowsVersionInfo(sb);
        AddExceptionInfo(sb, exception);
        
        var logFilePath = Path.Combine(crashLogsDirectory, fileName);
        
        File.WriteAllText(logFilePath, sb.ToString());
    }

    static void AddExceptionInfo(StringBuilder sb, Exception exception)
    {
        sb.AppendLine($"Unhandled {exception.GetType()}: {exception.Message}");
        sb.AppendLine($"Stack trace:\n{exception.StackTrace}");
    }

    static void AddWindowsVersionInfo(StringBuilder sb)
    {
        // The wonderful library for OS version detection is only available in .NET 4.8, which breaks installation, such a shame
        // Uncomment when https://github.com/pruggitorg/detect-windows-version/pull/27 is merged

        // sb.AppendLine($"Windows version: {OSVersion.GetOSVersion().Version.Major}.{OSVersion.GetOSVersion().Version.Minor}.{OSVersion.GetOSVersion().Version.Build}");
        // sb.AppendLine($"OS type: {OSVersion.GetOperatingSystem()}");
        // sb.AppendLine($"is workstation: {OSVersion.IsWorkstation}");
        // sb.AppendLine($"is server: {OSVersion.IsServer}");
        // sb.AppendLine($"64-Bit OS: {OSVersion.Is64BitOperatingSystem}");
        //
        // if (OSVersion.GetOSVersion().Version.Major >= 10)
        // {
        //     sb.AppendLine($"Windows Display Version: {OSVersion.MajorVersion10Properties().DisplayVersion ?? "(Unable to detect)"}");
        //     sb.AppendLine($"Windows Update Build Revision: {OSVersion.MajorVersion10Properties().UBR ?? "(Unable to detect)"}");
        // }
        //
        // sb.AppendLine("\n");
    }

    static void DisplayExceptionMessageBox(Exception exception)
    {
        var errorMessage =
            $"PowerShortcuts crashed, the exception has been logged.\n" +
            $"Please create an issue on the Github repository\n\n" +
            $"{exception.GetType()}: {exception.Message}";
        
        PInvoke.MessageBox(HWND.Null,
            errorMessage,
            "PowerShortcuts Unhandled Exception", MESSAGEBOX_STYLE.MB_OK | MESSAGEBOX_STYLE.MB_ICONERROR);
    }
}