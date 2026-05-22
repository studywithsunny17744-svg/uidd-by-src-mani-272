@echo off

setlocal EnableExtensions EnableDelayedExpansion



cd /d "%~dp0"

title Config + Build + Run



echo.

echo ============================================================

echo   STEP 1: branding.config se naam lagana

echo ============================================================

call "%~dp0scripts\Apply-Branding.bat"

if errorlevel 1 (

    echo [ERROR] branding.config check karo

    pause

    exit /b 1

)



echo.

echo ============================================================

echo   STEP 2: Build

echo ============================================================

set "SLN=%~dp0proxy-uid.sln"

set "MSBUILD="

if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"

if "!MSBUILD!"=="" (

    echo [ERROR] MSBuild not found

    pause

    exit /b 1

)



"!MSBUILD!" "!SLN!" /t:Rebuild /p:Configuration=Debug /v:minimal /nologo

if errorlevel 1 (

    echo [ERROR] Build failed

    pause

    exit /b 1

)



echo.

echo ============================================================

echo   STEP 3: App run

echo ============================================================

set "EXE="

for %%F in ("%~dp0bin\Debug\*.exe") do set "EXE=%%~fF"

if exist "!EXE!" (

    start "" "!EXE!"

    echo [OK] Started: !EXE!

) else (

    echo [WARN] EXE not found in bin\Debug

)



echo.

echo ============================================================

echo   GitHub push? scripts\GitHub-Push-Badges.bat chalao

echo ============================================================

echo.

pause

endlocal


