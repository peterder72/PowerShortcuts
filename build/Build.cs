using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.DotNet;
using ParameterAttribute = Nuke.Common.ParameterAttribute;


[GitHubActions("continuous",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = true,
    OnPushBranches = ["master"],
    InvokedTargets = new[]
    {
        nameof(UploadArtifacts),
    }
)]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    static string EntrypointProject = "PowerShortcuts.Host";
    static string InstallerProject = "PowerShortcuts.Installer";
    static string TargetRuntime = "win-x64";

    static AbsolutePath DeployDirectory = RootDirectory / "Deploy";
    static AbsolutePath BuildDirectory = DeployDirectory / "PowerShortcuts";
    static AbsolutePath InstallerDirectory = DeployDirectory / "PowerShortcutsInstaller";
    static AbsolutePath InstallerFile = InstallerDirectory / "PowerShortcuts.msi";

    Target Prepare => _ => _
        .Before(Publish)
        .Executes(() =>
        {
            DotNetToolInstall(_ => _
                .SetPackageName("wix")
                .SetGlobal(true));
        });
    
    Target Clean => _ => _
        .Before(Publish)
        .Executes(() =>
        {
            DotNetClean();
            
            if (BuildDirectory.DirectoryExists()) BuildDirectory.DeleteDirectory();
            if (InstallerDirectory.DirectoryExists()) InstallerDirectory.DeleteDirectory();
        });

    Target Publish => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetPublish(b => b
                .SetProject(EntrypointProject)
                .SetRuntime(TargetRuntime)
                .SetPublishReadyToRun(true)
                .SetConfiguration(Configuration)
                .SetPublishSingleFile(true)
                .SetOutput(BuildDirectory));
        });
    
    Target CreateInstaller => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            DotNetBuild(b => b
                .SetProjectFile(InstallerProject)
                .SetConfiguration(Configuration)
                .SetRuntime(TargetRuntime));
        });

    Target UploadArtifacts => _ => _
        .DependsOn(CreateInstaller)
        .Produces(BuildDirectory)
        .Produces(InstallerFile);
}
