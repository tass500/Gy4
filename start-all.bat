@echo off
setlocal enabledelayedexpansion

:: Configuration
set HTTP_PORT=5024
set HTTPS_PORT=7203
set FRONTEND_PORT=3000

:: Clear screen
cls

echo ===================================
echo  Starting All Services...
echo ===================================

:: Start Backend
echo [1/2] Starting Backend...
start "Backend" /B cmd /c ""%~dp0start-backend.bat""

echo Waiting for backend to initialize...
timeout /t 10 /nobreak >nul

:: Start Frontend
echo [2/2] Starting Frontend...
start "Frontend" /B cmd /c ""%~dp0start-frontend.bat""

:: Wait for services to start
timeout /t 5 /nobreak >nul

:: Clear screen and show status
cls
echo ===================================
echo  Services Status
echo ===================================
echo  [Backend]
echo  - URL:      http://localhost:%HTTP_PORT%
echo  - Swagger:  https://localhost:%HTTPS_PORT%/swagger
echo  - Status:   !(if exist "%TEMP%\backend_running" (echo RUNNING) else echo NOT RUNNING)!
echo.
echo  [Frontend]
echo  - URL:     http://localhost:%FRONTEND_PORT%
echo  - Status:  !(if exist "%TEMP%\frontend_running" (echo RUNNING) else echo NOT RUNNING)!
echo ===================================
echo.

:: Ask to open in browser
set /p openBrowser=Do you want to open in browser? (y/n): 
if /i "!openBrowser!"=="y" (
    echo Opening browser...
    start "" "https://localhost:%HTTPS_PORT%/swagger"
    timeout /t 2 /nobreak >nul
    start "" "http://localhost:%FRONTEND_PORT%"
) else (
    echo Browser not opened.
)

echo.
echo You can access the services at any time:
echo - Swagger UI:  https://localhost:%HTTPS_PORT%/swagger
echo - Frontend:    http://localhost:%FRONTEND_PORT%
echo.
pause
