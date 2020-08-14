cls
Write-Host "Innovt Platform PowerShell Build" -ForegroundColor "Green"

cd X:\Projects\Innovt.Platform\src

$packageOutputFolder = "$PSScriptRoot\.nupkgs"

mkdir -Force $packageOutputFolder | Out-Null

Remove-Item –path $packageOutputFolder\*

#projects 
$projectsToBuild =
   'Innovt.Cloud',
   'Innovt.Cloud.AWS',
   'Innovt.Cloud.AWS.Cognito',
   'Innovt.Cloud.AWS.Dynamo',
   'Innovt.Cloud.AWS.Lambda',
   'Innovt.Cloud.AWS.Notification',
   'Innovt.Cloud.AWS.S3',
   'Innovt.Cloud.AWS.SQS',
   'Innovt.Core',
   'Innovt.Job.Quartz',
   'Innovt.Job.Core',
   'Innovt.Cqrs',
   'Innovt.CrossCutting.IOC',
   'Innovt.CrossCutting.Log.Serilog',
   'Innovt.Data',
   'Innovt.Data.Ado',
   'Innovt.Data.EFCore',
   'Innovt.Domain',
   'Innovt.AspNetCore',
   'Innovt.Notification.Core'


function incrementVersion{ 

    param( $csProject );
        Write-Host $xml
    $xml = [Xml] (Get-Content $csProject)
      Write-Host $xml
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


Write-Host "Building all packages" -ForegroundColor "Green"

$packageVersion


foreach ($project in $projectsToBuild)
 {
    Write-Host "Working on $project`:" -ForegroundColor "Magenta"
	
	Push-Location ".\$project"

    if(!$packageVersion){
        $packageVersion =  incrementVersion("$PSScriptRoot\$project\$project.csproj")
    }
    
    Write-Host "  Restoring and packing" -NoNewline -ForegroundColor "Magenta"
   
    dotnet pack --output $packageOutputFolder -c Release -p:PackageVersion=$packageVersion

	Pop-Location

    Write-Host "Done." -ForegroundColor "Green"
  }

  Write-Host "Build Complete." -ForegroundColor "Green"
 
 
  Write-Host "Sending Packages" -NoNewline -ForegroundColor "Magenta"

 $packages = Get-ChildItem -Path $packageOutputFolder
  
foreach ($p in $packages)
{
    Write-Host "Sending Package $p :" -ForegroundColor "Magenta"

   $pkgPath = $packageOutputFolder +"\"+$p.Name; 
    ..\tools\nuget.exe push $pkgPath !1nn0vt# -Source http://nugetinnovt.azurewebsites.net/api/v2/package
}

Write-Host "Package Push Complete." -ForegroundColor "Green"