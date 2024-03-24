using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.DotNet;
using ParameterAttribute = Nuke.Common.ParameterAttribute;


[GitHubActions("continuous",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = true,
    On = [GitHubActionsTrigger.Push],
    InvokedTargets = new[]
    {
        nameof(Publish),
    }
)]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    static AbsolutePath DeployDirectory = RootDirectory / "Deploy";
    static AbsolutePath BuildDirectory = DeployDirectory / "PowerShortcuts";
    static AbsolutePath OutputArchive = DeployDirectory / "PowerShortcuts.zip";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean();
            
            if (BuildDirectory.DirectoryExists()) BuildDirectory.DeleteDirectory();
            if (OutputArchive.FileExists()) OutputArchive.DeleteFile();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore();
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(b => b
                .SetProjectFile("PowerShortcuts.Host")
                .SetConfiguration(Configuration)
                .SetOutputDirectory(BuildDirectory)
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() => BuildDirectory.CompressTo(OutputArchive))
        .Produces(OutputArchive);
}
