param([switch]$sign)

$baseOutputPath = "BuildResult"
$netCorePath = "$baseOutputPath\NetCore"
$netClassicPath = "$baseOutputPath\NetClassic"
$libPath = "lib"

$targetFrameworks = @{
 "\net40" = $netClassicPath;
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
  &$msbuildExe Cloudinary\Cloudinary.csproj /t:"Build" /p:"Configuration="Release";OutDir=$outPath;TargetFrameworkVersion="v4.0";Sign="$sign""

}


function Build-Net-Core($outPath) {

  dotnet build --configuration Release .\Core\CloudinaryDotNet.Core.csproj --output $outPath

}

function Create-Package-Structure($basePath, $targets) {
   
   Remove-Item $basePath -Force  -Recurse -ErrorAction SilentlyContinue
   New-Item -ItemType directory $basePath
   
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

}

Write-Host "Building cloudinary net classic library..."
$outBuildPath = "{0}{1}" -f  "..\", $netClassicPath
Build-Net-Classic $outBuildPath 
Write-Host "Building cloudinary net core library..."
$outBuildPath = "{0}{1}" -f  "..\", $netCorePath
Build-Net-Core $outBuildPath 

Create-Package-Structure $libPath $targetFrameworks
Create-Package

Write-Host "Done"
