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

    static string EntrypointProject = "PowerShortcuts.Host";

    static AbsolutePath DeployDirectory = RootDirectory / "Deploy";
    static AbsolutePath BuildDirectory = DeployDirectory / "PowerShortcuts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean();
            
            if (BuildDirectory.DirectoryExists()) BuildDirectory.DeleteDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(b => b.SetProjectFile(EntrypointProject));

        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(b => b
                .SetProjectFile(EntrypointProject)
                .SetConfiguration(Configuration)
                .SetSelfContained(true)
                .SetOutputDirectory(BuildDirectory));
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Produces(BuildDirectory);
}
