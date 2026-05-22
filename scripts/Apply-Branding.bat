@echo off
setlocal
cd /d "%~dp0.."
if not exist "branding.config" (
    echo [ERROR] branding.config missing in project folder.
    pause
    exit /b 1
)
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Apply-Branding.ps1" -Root "%CD%"
exit /b %ERRORLEVEL%

