Write-Host "Setting up appsettings.json for tests"

$strConfigSource = "$env:APPVEYOR_BUILD_FOLDER\CloudinaryDotNet.IntegrationTests\appsettings.json.sample"
$strConfigDest = "$env:APPVEYOR_BUILD_FOLDER\CloudinaryDotNet.IntegrationTests\appsettings.json"      
$apiEndpoint = "https://sub-account-testing.cloudinary.com/create_sub_account"

$postParams = @{
 "prefix"="DotNet"
} | ConvertTo-Json

$res = Invoke-WebRequest -Uri $apiEndpoint -ContentType "application/json" -Method POST -Body $postParams | ConvertFrom-Json

$cloud = $res.payload

Copy-Item $strConfigSource -Destination $strConfigDest

(Get-Content $strConfigDest).replace('"ApiBaseAddress": ""', '"ApiBaseAddress": "' + $env:ApiBaseAddress + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"CloudName": ""', '"CloudName": "' + $cloud.cloudName + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"ApiKey": ""', '"ApiKey": "'+ $cloud.cloudApiKey + '"') | Set-Content $strConfigDest
(Get-Content $strConfigDest).replace('"ApiSecret": ""', '"ApiSecret": "' + $cloud.cloudApiSecret + '"') | Set-Content $strConfigDest

Write-Host "CloudName: $($cloud.cloudName)"

Write-Host "Done"
