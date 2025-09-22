# Stop if any error occurs
$ErrorActionPreference = "Stop"

Write-Host "Restoring NuGet packages..." -ForegroundColor Cyan
& dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org --configfile .\nuget.config

Write-Host "Restoring project dependencies..." -ForegroundColor Cyan
& dotnet restore --no-cache --force

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to restore packages. Please check your network connection and try again."
    exit 1
}

Write-Host "Building the application..." -ForegroundColor Cyan
& dotnet build --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed. Please check the build errors above."
    exit 1
}

Write-Host "Starting the application..." -ForegroundColor Green
Write-Host "API will be available at: http://localhost:5023" -ForegroundColor Green
Write-Host "Swagger UI will be available at: http://localhost:5023/swagger" -ForegroundColor Green

& dotnet run
