param(
    [Parameter(Mandatory=$true)][string]$VersionStr
)

# Some basic validation
$Version = [System.Version]::Parse($VersionStr)

$CodeVersionPath = "$PSScriptRoot/CloudinaryDotNet/CloudinaryVersion.cs"

$CsProjPath = "$PSScriptRoot/CloudinaryDotNet/CloudinaryDotNet.csproj"

function Set-Version-In-Code($CodeVersionPath, $Version) {
    $Text = [IO.File]::ReadAllText($CodeVersionPath) -replace "Full => `"\d+\.\d+\.?\d*`"", "Full => `"$Version`""
    [IO.File]::WriteAllText($CodeVersionPath, $Text)
}

function Set-CsProj-Version($CsProjPath, $Version) {
    $Text = [IO.File]::ReadAllText($CsProjPath)
    $r = [regex]'<Version>\d+\.\d+\.?\d*</Version>'
    $Text = $r.Replace($Text, "<Version>$Version</Version>", 1) # Replaces only the first occurrence
    [IO.File]::WriteAllText($CsProjPath, $Text)
}

Set-Version-In-Code $CodeVersionPath $Version
Set-CsProj-Version $CsProjPath $Version

