param([switch]$sign)

Write-Host "Starting dotnet pack..."

dotnet pack -p:TreatWarningsAsErrors=false -c:Release -o:lib

Write-Host "Done"
