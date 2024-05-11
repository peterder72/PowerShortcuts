using System;
using System.IO;
using System.Linq;
using WixSharp;

namespace PowerShortcuts.Installer
{
    internal class Program
    {
        private const string AppName = "PowerShortcuts";
        private static readonly Version AppVersion = new Version(0, 1, 0);

        private const string MainExecutableName = "PowerShortcuts.Host.exe";

        private const string PowerShortcutsBuildRoot = @"..\Deploy\PowerShortcuts";
        private const string PowerShortcutsInstallDir = @"%LocalAppDataFolder%\Osetr\PowerShortcuts";
    
        private static readonly string[] ShortcutDirectories = { "%ProgramMenu%", "%Startup%" };

        public static void Main(string[] args)
        {
            var installDir = new Dir(PowerShortcutsInstallDir, new Files(Path.Combine(PowerShortcutsBuildRoot, "*.*")));
        
            var project = new Project(AppName, installDir);

            project.Version = AppVersion;

            project.OutDir = "../Deploy/PowerShortcutsInstaller";
            project.UI = WUI.WixUI_Minimal;

            project.InstallScope = InstallScope.perUser;
            project.GUID = new Guid("642cc5a9-eb76-4708-a619-3e3bd5dea645");

            var shortcutName = $"{AppName} {AppVersion}";
            var detachedArguments = "-d";

            project.ResolveWildCards().FindFile(f => f.Name.EndsWith(MainExecutableName))
                .First()
                .Shortcuts = ShortcutDirectories
                .Select(x => new FileShortcut(shortcutName, x)
                {
                    Arguments = detachedArguments
                })
                .ToArray(); 

            Compiler.BuildMsi(project);
        }
    }
}