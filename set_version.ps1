param(
    [Parameter(Mandatory=$true)][string]$VersionStr
)

# Some basic validation
$Version = [System.Version]::Parse($VersionStr)

$NuspecPath = "$PSScriptRoot/CloudinaryDotNet.nuspec"

$CsProjPaths = @(
    "$PSScriptRoot/Cloudinary/Cloudinary.csproj",
    "$PSScriptRoot/Core/CloudinaryDotNet.Core.csproj"
)

function Set-Nuspec-Version($NuspecPath, $Version) {
    $Text = [IO.File]::ReadAllText($NuspecPath) -replace "<version>\d+\.\d+\.?\d*</version>", "<version>$Version</version>"
    [IO.File]::WriteAllText($NuspecPath, $Text)
}

function Set-CsProj-Version($CsProjPath, $Version) {
    $Text = [IO.File]::ReadAllText($CsProjPath) `
        -replace "<Version>\d+\.\d+\.?\d*</Version>", "<Version>$Version</Version>" `
        -replace "<FileVersion>\d+\.\d+\.?\d*</FileVersion>", "<FileVersion>$Version</FileVersion>"
    [IO.File]::WriteAllText($CsProjPath, $Text)
}

Set-Nuspec-Version $NuspecPath $Version

foreach ($CsProjPath in $CsProjPaths) {
	Set-CsProj-Version $CsProjPath $Version
}
