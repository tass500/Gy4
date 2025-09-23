# Start Frontend Script
Write-Host "Starting Frontend..."

# Navigate to the frontend directory
$frontendPath = "c:\Users\zolta\CascadeProjects\Gyakorlas\Gy4\src\frontend"
Set-Location $frontendPath

# Install dependencies if node_modules doesn't exist
if (-not (Test-Path "node_modules")) {
    Write-Host "Installing dependencies..."
    npm install
}

# Start the frontend
Write-Host "Starting Frontend on port 3000..."
Start-Process -NoNewWindow -FilePath "npm" -ArgumentList "start" -WorkingDirectory $frontendPath

Write-Host "Frontend is starting..."
Write-Host "Application will be available at: http://localhost:3000"
