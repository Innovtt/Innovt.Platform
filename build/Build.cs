using System;
using NuGet.Common;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    [Parameter] string NugetApiUrl = "http://nugetinnovt.azurewebsites.net/api/v2/package";
    [Parameter] string NugetApiKey;


    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Compile => _ => _
        .DependsOn(Clean).After()
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetAuthors("Michel Borges & Tiago Freire & Welbert Serra"));
        });


    Target Pack => _ => _
        .DependsOn(Compile).After()
        .Executes(() =>
        {
            DotNetPack(p => p
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetAuthors("Michel Borges & Tiago Freire & Welbert Serra")
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetNoDependencies(true)
                .SetOutputDirectory(ArtifactsDirectory / "nuget")
            );
        });

    Target Publish => _ => _
        .DependsOn(Pack).After()
        .Requires(() => NugetApiUrl)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration == "Release")
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory / "nuget", "*.nupkg")
                .NotNull()
                // .Where(x => x.StartsWith("Innovt.",StringComparison.InvariantCultureIgnoreCase))
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .EnableSkipDuplicate()
                        .SetTargetPath(x)
                        .SetSource(NugetApiUrl)
                        .SetApiKey(NugetApiKey)
                    );
                });
        });
}