using PowerShortcuts.Core.Composition;
using PowerShortcuts.WinService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddPowerShortcutsCore();
builder.Services.AddHostedService<WindowsBackgroundService>();

using var host = builder.Build();

host.Run();