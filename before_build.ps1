Write-Host "Copying appsettings.json for tests"

$strConfigSource = "c:\projects\cloudinary\Shared.Tests\appsettings.json.sample"
$strConfigDest = "c:\projects\cloudinary\Shared.Tests\appsettings.json"   

Copy-Item $strConfigSource -Destination $strConfigDest
(Get-Content $strConfigDest).replace('"ApiBaseAddress": ""', '"ApiBaseAddress": "' + $env:ApiBaseAddress + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"CloudName": ""', '"CloudName": "' + $env:CloudName + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"ApiKey": ""', '"ApiKey": "'+ $env:ApiKey + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"ApiSecret": ""', '"ApiSecret": "' + $env:ApiSecret + '"') | Set-Content $strConfigDest
