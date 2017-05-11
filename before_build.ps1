Write-Host "Copying App.config for tests"

$strConfigSource = "c:\projects\cloudinary\Cloudinary.Test\app.config.sample"
$strConfigDest = "c:\projects\cloudinary\Cloudinary.Test\app.config"   

Copy-Item $strConfigSource -Destination $strConfigDest
(Get-Content $strConfigDest).replace('ApiBaseAddress=""', 'ApiBaseAddress="' + $env:ApiBaseAddress + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('CloudName=""', 'CloudName="' + $env:CloudName + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('ApiKey=""', 'ApiKey="'+ $env:ApiKey + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('ApiSecret=""', 'ApiSecret="' + $env:ApiSecret + '"') | Set-Content $strConfigDest