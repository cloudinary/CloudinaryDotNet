param([switch]$sign)

$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"

&$msbuildExe Cloudinary\Cloudinary.csproj /t:"Build" /p:"Configuration="Release";TargetFrameworkVersion="v3.5";Sign="$sign""
&$msbuildExe Cloudinary\Cloudinary.csproj /t:"Build" /p:"Configuration="Release";TargetFrameworkVersion="v4.0";Sign="$sign""
.nuget\NuGet.exe pack CloudinaryDotNet.nuspec