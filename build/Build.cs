// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: _build
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly string Configuration = IsLocalBuild ? "Debug" : "Release";

    //[GitRepository] readonly GitRepository GitRepository;
    [GitVersion(Framework = "netcoreapp3.0")] 
    readonly GitVersion GitVersion;


    [Solution] readonly Solution Solution;
    [Parameter] string NugetApiKey;

    [Parameter] string NugetApiUrl = "http://nugetinnovt.azurewebsites.net/api/v2/package";

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetCopyright("Innovt Technologies")
                .SetAuthors("Michel Borges & Tiago Freire & Welbert Serra")
                .EnableNoRestore());
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
            DotNetPack(p => p
                    .SetProject(Solution)
                    .SetAuthors("Michel Borges & Tiago Freire & Welbert Serra")
                    .SetVersion(GitVersion.NuGetVersionV2)
                    .SetNoDependencies(true)
                    .SetOutputDirectory(ArtifactsDirectory / "nuget")
                //.SetRepositoryType("git")
                // .SetRepositoryUrl("https://github.com/Innovtt/Innovt.Platform")
            );
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Requires(() => NugetApiUrl)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.Equals("Release", StringComparison.InvariantCultureIgnoreCase))
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory / "nuget", "*.nupkg")
                .NotEmpty()                
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

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Publish);
}