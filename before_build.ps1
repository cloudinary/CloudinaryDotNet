Write-Host "Setting up appsettings.json for tests"

$strConfigSource = "$env:APPVEYOR_BUILD_FOLDER\Shared.IntegrationTests\appsettings.json.sample"
$strConfigDest = "$env:APPVEYOR_BUILD_FOLDER\Shared.IntegrationTests\appsettings.json"      

Copy-Item $strConfigSource -Destination $strConfigDest

(Get-Content $strConfigDest).replace('"ApiBaseAddress": ""', '"ApiBaseAddress": "' + $env:ApiBaseAddress + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"CloudName": ""', '"CloudName": "' + $env:CloudName + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"ApiKey": ""', '"ApiKey": "'+ $env:ApiKey + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"ApiSecret": ""', '"ApiSecret": "' + $env:ApiSecret + '"') | Set-Content $strConfigDest

Write-Host "Done"