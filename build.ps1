param([switch]$sign)

$ErrorActionPreference = "Stop"

$baseOutputPath = "BuildResult"
$netCorePath = "$baseOutputPath\NetCore"
$netClassicPath = "$baseOutputPath\NetClassic"
$libPath = "lib"

$targetFrameworks = @{
 "\net45" = $netClassicPath;
 "\netstandard1.3" = $netCorePath;
 "\netcore" = $netCorePath;
}


function Get-MSBuild-Path {

    $vs14key = "HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"
    $vs15key = "HKLM:\SOFTWARE\wow6432node\Microsoft\VisualStudio\SxS\VS7"

    $msbuildPath = ""

    if (Test-Path $vs14key) {
        $key = Get-ItemProperty $vs14key
        $subkey = $key.MSBuildToolsPath
        if ($subkey) {
            $msbuildPath = Join-Path $subkey "msbuild.exe"
        }
    }

    if (Test-Path $vs15key) {
        $key = Get-ItemProperty $vs15key
        $subkey = $key."15.0"
        if ($subkey) {
            $msbuildPath = Join-Path $subkey "MSBuild\15.0\bin\amd64\msbuild.exe"
        }
    }

    return $msbuildPath
    
}

function Build-Net-Classic($outPath) {
    
  $msbuildExe = Get-MSBuild-Path
  &$msbuildExe Cloudinary\Cloudinary.csproj /t:"Clean,Build" /p:"Configuration="Release";OutDir=$outPath;TargetFrameworkVersion="v4.5";Sign="$sign""
  if (-not $?) { exit 1 }
}


function Build-Net-Core($outPath) {

  dotnet build --no-incremental --configuration Release .\Core\CloudinaryDotNet.Core.csproj --output $outPath /p:"Sign="$sign""
  if (-not $?) { exit 1 }
}

function Create-Package-Structure($basePath, $targets) {
   
   Cleanup-Folder $basePath
   
   ForEach ($target in $targets.GetEnumerator()) 
   {
        $targetPath = "{0}{1}" -f  $basePath, $target.Key
        $destPath = "{0}\*.*" -f  $target.Value
        New-Item -ItemType directory $targetPath
        Copy-Item -Path $destPath -Destination $targetPath -Recurse
   }
}

function Create-Package {
  
  .nuget\NuGet.exe pack CloudinaryDotNet.nuspec
  if (-not $?) { exit 1 }
}

function Cleanup-Folder ($folder) {
    if (Test-Path -Path "$folder") {
        Write-Host "Cleaning up folder: $folder"
        Remove-Item "$folder\*" -Force -Recurse
    } else {
         Write-Host "Folder: $folder does not exist, creating"
         New-Item -ItemType directory $folder
    }
}


Cleanup-Folder $baseOutputPath
Cleanup-Folder $netClassicPath

$netClassicPath = Resolve-Path -Path $netClassicPath
Write-Host "Building Cloudinary .NET Classic library to: $netClassicPath"
Build-Net-Classic $netClassicPath 

Cleanup-Folder $netCorePath

$netCorePath = Resolve-Path -Path $netCorePath
Write-Host "Building Cloudinary .NET Core library to: $netCorePath"
Build-Net-Core $netCorePath 

Create-Package-Structure $libPath $targetFrameworks
Create-Package

Write-Host "Done"
