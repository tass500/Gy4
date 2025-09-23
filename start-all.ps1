# Start All Services Script
Write-Host "Starting all services..."

# Start Backend
$backendScript = Join-Path $PSScriptRoot "start-backend.ps1"
Start-Process -NoNewWindow -FilePath "powershell" -ArgumentList "-NoExit", "-File", "`"$backendScript`""

# Wait a moment for backend to start
Start-Sleep -Seconds 5

# Start Frontend
$frontendScript = Join-Path $PSScriptRoot "start-frontend.ps1"
Start-Process -NoNewWindow -FilePath "powershell" -ArgumentList "-NoExit", "-File", "`"$frontendScript`""

Write-Host "All services are starting..."
Write-Host "- Backend: http://localhost:5000"
Write-Host "- Frontend: http://localhost:3000"
