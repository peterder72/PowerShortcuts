// See https://aka.ms/new-console-template for more information

using PowerShortcuts.Core.Composition;
using PowerShortcuts.Core.Interface;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddPowerShortcutsCore();

using var app = builder.Build();

var service = app.Services.GetRequiredService<IPowerShortcutsService>();

service.Initialize();

app.Run();