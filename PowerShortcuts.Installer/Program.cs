using System;
using WixSharp;

internal class Program
{
    public static void Main(string[] args)
    {
        var project = new Project("PowerShortcuts",
            new Dir(@"%LocalAppDataFolder%\Osetr\PowerShortcuts",
                new File(@"../Deploy/PowerShortcuts/PowerShortcuts.Host.exe"),
                new File(@"../Deploy/PowerShortcuts/VirtualDesktopAccessor.dll")
            ));

        project.Version = new Version(0, 0, 2);

        project.OutDir = "../Deploy/PowerShortcutsInstaller";
        project.UI = WUI.WixUI_Minimal;

        project.InstallScope = InstallScope.perUser;
        project.GUID = new Guid("642cc5a9-eb76-4708-a619-3e3bd5dea645");

        Compiler.BuildMsi(project);
    }
}