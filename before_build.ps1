Write-Host "Copying App.config for tests"

Write-Host $ApiBaseAddress
Write-Host $CloudName
Write-Host $ApiKey
Write-Host $ApiSecret

$strConfigSource = "d:\install\SED\app.config.sample"
$strConfigDest = "d:\install\SED\app.config"   

Copy-Item $strConfigSource -Destination $strConfigDest
(Get-Content $strConfigDest).replace('ApiBaseAddress=""', 'ApiBaseAddress="' + $ApiBaseAddress + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('CloudName=""', 'CloudName="' + $CloudName + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('ApiKey=""', 'ApiKey="'+ $ApiKey + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('ApiSecret=""', 'ApiSecret="' + $ApiSecret + '"') | Set-Content $strConfigDest