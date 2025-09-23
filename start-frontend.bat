@echo off
setlocal enabledelayedexpansion

:: Configuration
set FRONTEND_PORT=3000

:: Create a status file
echo 1>"%TEMP%\frontend_running"

:: Stop any existing Node.js processes
taskkill /f /im node.exe >nul 2>&1

:: Change to the frontend directory
cd /d "%~dp0src\frontend"

:: Display status
cls
echo ===================================
echo  Frontend Service
echo ===================================
echo Starting frontend...
echo.
echo Configuration:
echo - URL: http://localhost:%FRONTEND_PORT%
echo.

:: Install dependencies if needed
if not exist "node_modules" (
    echo Installing dependencies...
    call npm install
    if !ERRORLEVEL! neq 0 (
        echo Error: Failed to install dependencies!
        del "%TEMP%\frontend_running" >nul 2>&1
        pause
        exit /b !ERRORLEVEL!
    )
)

echo ===================================
echo Starting Angular development server...
echo ===================================
echo.

:: Start the Angular development server
call ng serve --port %FRONTEND_PORT% --open

:: Cleanup if the service stops
del "%TEMP%\frontend_running" >nul 2>&1
