@echo off
setlocal enabledelayedexpansion

:: Configuration
set HTTP_PORT=5024
set HTTPS_PORT=7203

:: Create a status file
echo 1>"%TEMP%\backend_running"

:: Stop any existing backend processes
taskkill /f /im dotnet.exe /fi "WINDOWTITLE eq Backend" >nul 2>&1
taskkill /f /im ApiGateway.exe >nul 2>&1
taskkill /f /im dotnet.exe >nul 2>&1

:: Change to the backend directory
cd /d "%~dp0src\api-gateway"

:: Set environment variables
set ASPNETCORE_URLS=http://localhost:%HTTP_PORT%;https://localhost:%HTTPS_PORT%
set ASPNETCORE_ENVIRONMENT=Development

:: Display status
cls
echo ===================================
echo  Backend Service
echo ===================================
echo Starting backend...
echo.
echo Configuration:
echo - Environment: Development
echo - HTTP:  http://localhost:%HTTP_PORT%
echo - HTTPS: https://localhost:%HTTPS_PORT%
echo - Swagger: https://localhost:%HTTPS_PORT%/swagger
echo.
echo Building and starting the backend...
echo ===================================
echo.

:: Build and run the backend
dotnet build
if %ERRORLEVEL% neq 0 (
    echo Error: Build failed!
    del "%TEMP%\backend_running" >nul 2>&1
    pause
    exit /b %ERRORLEVEL%
)

:: Ensure the application is running in Development environment
dotnet run --environment=Development --urls="http://localhost:%HTTP_PORT%;https://localhost:%HTTPS_PORT%"

:: Cleanup if the service stops
del "%TEMP%\backend_running" >nul 2>&1
