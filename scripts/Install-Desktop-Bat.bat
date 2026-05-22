@echo off
setlocal
chcp 65001 >nul 2>&1

set "ROOT=%~dp0.."
set "SCRIPTS=%~dp0"
set "DESKTOP=%USERPROFILE%\Desktop"
set "DESK_BAT=%DESKTOP%\TRYHARD - GitHub Badges Push.bat"

echo Creating Desktop runner...
(
echo @echo off
echo title TRYHARD — GitHub Badges Push
echo cd /d "%SCRIPTS%"
echo call "%SCRIPTS%GitHub-Push-Badges.bat"
) > "%DESK_BAT%"

if not exist "%DESK_BAT%" (
    echo [ERROR] Could not write: %DESK_BAT%
    pause
    exit /b 1
)

echo.
echo [OK] Desktop BAT created:
echo      %DESK_BAT%
echo.
echo Double-click it on Desktop to:
echo   - Build + Run app
echo   - Push project in chunks to GitHub
echo   - Open repo so badges go live
echo.
set /p RUN=Run it now? [Y/n]: 
if /i "%RUN%"=="n" goto :done
call "%DESK_BAT%"

:done
pause
endlocal
