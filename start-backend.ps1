# Start Backend Script
Write-Host "Starting Backend Services..."

# Navigate to the backend directory
$backendPath = "c:\Users\zolta\CascadeProjects\Gyakorlas\Gy4\src\api-gateway"
Set-Location $backendPath

# Start the backend
Write-Host "Starting API Gateway on port 5000..."
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory $backendPath

Write-Host "Backend services are starting..."
Write-Host "API Gateway will be available at: http://localhost:5000"
Write-Host "Swagger UI will be available at: http://localhost:5000/swagger"
