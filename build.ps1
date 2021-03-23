param([switch]$sign)

Write-Host "Starting dotnet pack..."

dotnet pack -c:Release -o:lib -p:Sign=$sign

Write-Host "Done"
