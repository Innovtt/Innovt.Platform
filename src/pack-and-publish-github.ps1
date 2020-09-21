cls
Write-Host "Innovt Platform PowerShell Build" -ForegroundColor "Green"

cd $PSScriptRoot

$packageOutputFolder = "$PSScriptRoot\.nupkgs"

mkdir -Force $packageOutputFolder | Out-Null

Remove-Item –path $packageOutputFolder\*

#projects 
$projectsToBuild =
        'Innovt.Cloud',
        'Innovt.AspNetCore',
        'Innovt.Cloud.AWS',
        'Innovt.Cloud.AWS.Cognito',
        'Innovt.Cloud.AWS.Dynamo',
        'Innovt.Cloud.AWS.Kinesis',
        'Innovt.Cloud.AWS.Lambda',
        'Innovt.Cloud.AWS.Lambda.Kinesis',
        'Innovt.Cloud.AWS.Lambda.S3',
        'Innovt.Cloud.AWS.Lambda.Sqs',
        'Innovt.Cloud.AWS.Notification',
        'Innovt.Cloud.AWS.S3',
        'Innovt.Cloud.AWS.Sqs',
        'Innovt.Core',
        'Innovt.Cqrs',
        'Innovt.CrossCutting.IOC',
        'Innovt.CrossCutting.Log.Serilog',
        'Innovt.Data',
        'Innovt.Data.Ado',
        'Innovt.Data.EFCore',
        'Innovt.Domain',
        'Innovt.Job.Core',
        'Innovt.Job.Quartz',
        'Innovt.Notification.Core'


function incrementVersion{ 

    param( $csProject );
        
    $xml = [Xml] (Get-Content $csProject)

    $initialVersion = [Version] $xml.Project.PropertyGroup.PackageVersion

    Write-host 'initial version'

    Write-host $initialVersion

    #Get the build number (number of days since January 1, 2000)
    $baseDate = [datetime]"11/01/2019"
    $currentDate = $(Get-Date)
    $interval = (NEW-TIMESPAN -Start $baseDate -End $currentDate)
    $buildNumber = $interval.Days

    #Get the revision number (number seconds (divided by two) into the day on which the compilation was performed)
    $StartDate=[datetime]::Today
    $EndDate=(GET-DATE)
    $revisionNumber = [math]::Round((New-TimeSpan -Start $StartDate -End $EndDate).TotalSeconds / 2,0)

    #Final version number
    $finalBuildVersion = "$($initialVersion.Major).$($initialVersion.Minor).$($buildNumber).$($revisionNumber)"
    
    Write-host 'final version'

    Write-Host $finalBuildVersion;

    $finalBuildVersion;
}



function buildPackages{ 

    Write-Host "Building all packages" -ForegroundColor "Green"

    $packageVersion

    foreach ($project in $projectsToBuild)
     {
        Write-Host "Working on $project`:" -ForegroundColor "Magenta"
	
	    Push-Location ".\$project"

        if(!$packageVersion){
            $packageVersion =  incrementVersion("$PSScriptRoot\$project\$project.csproj")
        }
    
        Write-Host "Restoring and packing" -NoNewline -ForegroundColor "Magenta"
   
        dotnet pack --output $packageOutputFolder -c Release -p:PackageVersion=$packageVersion


	    Pop-Location

        Write-Host "Done." -ForegroundColor "Green"
      }
  
    Write-Host "Build Complete." -ForegroundColor "Green"

}

function publishPackages{ 
    Write-Host "Sending Packages" -NoNewline -ForegroundColor "Magenta"

    $packages = Get-ChildItem -Path $packageOutputFolder
  
    foreach ($p in $packages)
    {
        Write-Host "Sending Package $p :" -ForegroundColor "Magenta"

       $pkgPath = $packageOutputFolder +"\"+$p.Name; 
   
      ..\tools\nuget.exe push $pkgPath !1nn0vt# -Source http://nugetinnovt.azurewebsites.net/api/v2/package
      # dotnet nuget push $pkgPath --api-key $env:gh_innovt_token --source https://nuget.pkg.github.com/Innovtt/index.json --skip-duplicate --no-symbols true
    }

    Write-Host "Package Push Complete." -ForegroundColor "Green"
}

buildPackages;

publishPackages;
