using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WixSharp;

namespace PowerShortcuts.Installer
{
    internal class Program
    {
        private const string AppName = "PowerShortcuts";

        public const string MainExecutableName = "PowerShortcuts.Host.exe";

        private const string PowerShortcutsBuildRoot = @"..\Deploy\PowerShortcuts";
        private const string PowerShortcutsInstallDir = @"%LocalAppDataFolder%\Osetr\PowerShortcuts";

        private const string LicenseMarkdownPath = "../LICENSE.md";
        private const string LicenseRtfPath = "LICENSE.rtf";

        private static readonly string[] ShortcutDirectories = { "%ProgramMenu%", "%Startup%" };

        public static void Main()
        {
            var installDir = new Dir(PowerShortcutsInstallDir, new Files(Path.Combine(PowerShortcutsBuildRoot, "*.*")));

            var exePath = Path.Combine(PowerShortcutsBuildRoot, MainExecutableName);
            var version = GetVersionFromFile(exePath);

            MarkdownToRtfLicenseGenerator.ConvertMarkdownToRtf(LicenseMarkdownPath, LicenseRtfPath);

            var project = new Project(AppName, installDir);

            project.Version = version;
            project.LicenceFile = LicenseRtfPath;
            project.SourceBaseDir = Environment.CurrentDirectory;

            project.OutDir = "../Deploy/PowerShortcutsInstaller";
            project.OutFileName = $"{AppName}Installer_{version}";
            project.UI = WUI.WixUI_Minimal;
            project.MajorUpgrade = new MajorUpgrade
            {
                DowngradeErrorMessage = $"A later version of {AppName} is already installed. Setup will now exit.",
                AllowSameVersionUpgrades = true,
                AllowDowngrades = false
            };

            project.InstallScope = InstallScope.perUser;
            project.GUID = new Guid("642cc5a9-eb76-4708-a619-3e3bd5dea645");

            var shortcutName = $"{AppName} {version}";
            var detachedArguments = "-d";

            project.ResolveWildCards().FindFile(f => f.Name.EndsWith(MainExecutableName))
                .First()
                .Shortcuts = ShortcutDirectories
                .Select(x => new FileShortcut(shortcutName, x)
                {
                    Arguments = detachedArguments
                })
                .ToArray();


            project.ControlPanelInfo.Comments = "PowerShortcuts utility";
            project.ControlPanelInfo.Readme = "https://github.com/peterder72/PowerShortcuts";
            // project.ControlPanelInfo.ProductIcon = "app_icon.ico";
            project.ControlPanelInfo.Contact = "peterder72";
            project.ControlPanelInfo.Manufacturer = "peterder72";
            project.ControlPanelInfo.InstallLocation = "[INSTALLDIR]";
            project.ControlPanelInfo.NoModify = true;

            Compiler.BuildMsi(project);
        }

        private static Version GetVersionFromFile(string path)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(path);
            return new Version(versionInfo.FileVersion);
        }
    }
}