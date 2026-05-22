@echo off
setlocal EnableExtensions EnableDelayedExpansion
chcp 65001 >nul 2>&1
title EXE Builder — Config + Build

set "ROOT=%~dp0.."
set "CONFIG=%~dp0exe-builder.config.json"
cd /d "%ROOT%"

if not exist "%CONFIG%" (
  echo [ERROR] Missing scripts\exe-builder.config.json
  exit /b 1
)

echo [1/5] Reading config...
set "N=0"
for /f "usebackq delims=" %%A in (`powershell -NoProfile -Command "$c=Get-Content -Raw '%CONFIG%'|ConvertFrom-Json; Write-Output $c.appTitle; Write-Output $c.appSubtitle; Write-Output $c.assemblyName; Write-Output $c.exeOutputName"`) do (
  set /a N+=1
  if !N!==1 set "APP_TITLE=%%A"
  if !N!==2 set "APP_SUB=%%A"
  if !N!==3 set "ASM_NAME=%%A"
  if !N!==4 set "EXE_NAME=%%A"
)

echo   Title: !APP_TITLE!
echo   EXE:   !EXE_NAME!

echo [2/5] Patching AppBranding + csproj...
powershell -NoProfile -Command ^
  "$c=Get-Content -Raw '%CONFIG%'|ConvertFrom-Json;" ^
  "$exeName=($c.exeOutputName -replace '\.exe$','').Trim();" ^
  "$b=Get-Content -Raw 'Core\AppBranding.cs';" ^
  "$b=$b -replace 'AppTitle = \"[^\"]*\"',('AppTitle = \"'+$c.appTitle+'\"');" ^
  "$b=$b -replace 'AppSubtitle = \"[^\"]*\"',('AppSubtitle = \"'+$c.appSubtitle+'\"');" ^
  "Set-Content 'Core\AppBranding.cs' $b -Encoding UTF8;" ^
  "$p=Get-Content -Raw 'proxy-uid.csproj';" ^
  "$p=$p -replace '<AssemblyName>[^<]*</AssemblyName>',('<AssemblyName>'+$exeName+'</AssemblyName>');" ^
  "Set-Content 'proxy-uid.csproj' $p -Encoding UTF8"

echo [3/5] Finding MSBuild (GitHub Actions / VS)...
set "MSBUILD="
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
  for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe 2^>nul`) do set "MSBUILD=%%i"
)
if not defined MSBUILD if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if not defined MSBUILD (
  where msbuild >nul 2>&1 && set "MSBUILD=msbuild"
)
if not defined MSBUILD (
  echo [ERROR] MSBuild not found — workflow me setup-msbuild chahiye
  exit /b 1
)
echo   MSBuild: !MSBUILD!

echo [4/5] Restore + Rebuild Release...
"%MSBUILD%" "%ROOT%proxy-uid.sln" /t:Rebuild /p:Configuration=Release /v:minimal /nologo /restore
if errorlevel 1 (
  echo [WARN] Release failed, trying Debug...
  "%MSBUILD%" "%ROOT%proxy-uid.sln" /t:Rebuild /p:Configuration=Debug /v:minimal /nologo /restore
  if errorlevel 1 (
    echo [ERROR] MSBuild failed — NuGet packages / solution check karo
    exit /b 1
  )
  set "BIN_DIR=%ROOT%bin\Debug"
) else (
  set "BIN_DIR=%ROOT%bin\Release"
)

set "BUILT="
if exist "!BIN_DIR!\!EXE_NAME!" set "BUILT=!BIN_DIR!\!EXE_NAME!"
if not defined BUILT for %%F in ("!BIN_DIR!\*.exe") do set "BUILT=%%~fF" & goto :found
if not defined BUILT for %%F in ("%ROOT%bin\Release\*.exe") do set "BUILT=%%~fF" & goto :found
if not defined BUILT for %%F in ("%ROOT%bin\Debug\*.exe") do set "BUILT=%%~fF" & goto :found

:found
if not defined BUILT (
  echo [ERROR] bin\Release ya bin\Debug me EXE nahi mila
  dir "%ROOT%bin" /s /b *.exe 2>nul
  exit /b 1
)

echo [5/5] EXE ready
echo [OK] !BUILT!
exit /b 0
