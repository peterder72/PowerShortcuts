using System.Diagnostics;
using System.Reflection;
using PowerShortcuts.Core.Composition;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using PowerShortcuts.Host;
using PowerShortcuts.Host.Composition;
using PowerShortcuts.Host.Interface;
using PowerShortcuts.Host.Singleton;

UnhandledExceptionLogger.Initialize();

if (args is ["-d"] or ["--detach"])
{
    CreateDetachedProcess();
    Environment.Exit(0);
}

const string mutexName = "Osetr-PowerShortcuts-018f682f-cf2c-78f1-a511-f99399ceb266";
using var singletonLock = SingletonOperations.SingleInstanceLock(mutexName);

if (singletonLock.AnotherInstanceRunning)
{
    DisplayStartupErrorMessageBox("Another instance of PowerShortcuts is already running");
    Environment.Exit(42);
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddBackgroundService();
builder.Services.AddPowerShortcutsCore();
builder.Services.AddPowerShortcutsTray();

builder.Logging.ClearProviders();

builder.Logging
    .SetMinimumLevel(
#if DEBUG
    LogLevel.Debug
#else
    LogLevel.Information
#endif
)
    .AddConsole();

using var host = builder.Build();

var events = host.Services.GetRequiredService<IHostSystemEvents>();

var tokenSource = new CancellationTokenSource();

events.ExitRequestedObservable.Subscribe(_ => tokenSource.Cancel());

await host.RunAsync(tokenSource.Token);
return;

static void DisplayStartupErrorMessageBox(string message)
{
    PInvoke.MessageBox(HWND.Null,
        message,
        "PowerShortcuts Startup Error", MESSAGEBOX_STYLE.MB_OK | MESSAGEBOX_STYLE.MB_ICONERROR);
}

static void CreateDetachedProcess()
{
    var ownLocation = Assembly.GetEntryAssembly()?.Location ??
                      throw new InvalidOperationException("Could not get own location");
    var workingDirectory = Path.GetDirectoryName(ownLocation) ??
                           throw new InvalidOperationException("Could not get working directory");

    var process = new Process();
    ProcessStartInfo startInfo;

    if (ownLocation.EndsWith(".dll"))
    {
        // Always published with bundled .net runtime
        // and no guarantee that dotnet is in PATH
        // so we need to start the process with the .exe (which is always there)
        ownLocation = ownLocation.Replace(".dll", ".exe");
    }
    
    if (ownLocation.EndsWith(".exe"))
    {
        startInfo = new ProcessStartInfo
        {
            FileName = ownLocation,
            Arguments = string.Empty,
        };
    }
    else throw new InvalidOperationException("Unknown file type");

    startInfo.UseShellExecute = true;
    startInfo.WorkingDirectory = workingDirectory;
    startInfo.WindowStyle = ProcessWindowStyle.Hidden;

    process.StartInfo = startInfo;

    process.Start();
}