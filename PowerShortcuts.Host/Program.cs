using System.Diagnostics;
using System.Reflection;
using PowerShortcuts.Core.Composition;
using PowerShortcuts.WinService;
using PowerShortcuts.WinService.Composition;
using PowerShortcuts.WinService.Interface;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using PowerShortcuts.WinService.Singleton;

if (args is ["-d"] or ["--detach"])
{
    try
    {
        CreateDetachedProcess();
    }
    catch (Exception e)
    {
        DisplayStartupErrorMessageBox("Could not detach process: " + e.Message);
        throw;
    }
    
    Environment.Exit(0);
}

using var singletonLock = SingletonOperations.SingleInstanceLock("PowerShortcuts");

if (singletonLock.AnotherInstanceRunning)
{
    DisplayStartupErrorMessageBox("Another instance of PowerShortcuts is already running");
    Environment.Exit(42);
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddBackgroundService();
builder.Services.AddPowerShortcutsCore();
builder.Services.AddPowerShortcutsTray();

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
        startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"\"{ownLocation}\""
        };
    }
    else if (ownLocation.EndsWith(".exe"))
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