param([switch]$sign)

$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"
$msbuildExe = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe'
&$msbuildExe Cloudinary\Cloudinary.csproj /t:"Build" /p:"Configuration="Release";TargetFrameworkVersion="v4.0";Sign="$sign""
dotnet build --configuration Release .\Core\CloudinaryDotNet.Core.csproj
.nuget\NuGet.exe pack CloudinaryDotNet.nuspec