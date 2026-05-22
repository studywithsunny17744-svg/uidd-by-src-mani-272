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

echo [1/4] Reading config...
for /f "delims=" %%A in ('powershell -NoProfile -Command "$c=Get-Content -Raw '%CONFIG%'|ConvertFrom-Json; Write-Output $c.appTitle; Write-Output $c.appSubtitle; Write-Output $c.assemblyName; Write-Output $c.exeOutputName"') do (
  set /a N+=1
  if !N!==1 set "APP_TITLE=%%A"
  if !N!==2 set "APP_SUB=%%A"
  if !N!==3 set "ASM_NAME=%%A"
  if !N!==4 set "EXE_NAME=%%A"
)

echo   Title: !APP_TITLE!
echo   EXE:   !EXE_NAME!

echo [2/4] Patching AppBranding + csproj...
powershell -NoProfile -Command ^
  "$c=Get-Content -Raw '%CONFIG%'|ConvertFrom-Json;" ^
  "$b=Get-Content -Raw 'Core\AppBranding.cs';" ^
  "$b=$b -replace 'AppTitle = \"[^\"]*\"',('AppTitle = \"'+$c.appTitle+'\"');" ^
  "$b=$b -replace 'AppSubtitle = \"[^\"]*\"',('AppSubtitle = \"'+$c.appSubtitle+'\"');" ^
  "Set-Content 'Core\AppBranding.cs' $b -Encoding UTF8;" ^
  "$p=Get-Content -Raw 'proxy-uid.csproj';" ^
  "$p=$p -replace '<AssemblyName>[^<]*</AssemblyName>',('<AssemblyName>'+$c.assemblyName+'</AssemblyName>');" ^
  "Set-Content 'proxy-uid.csproj' $p -Encoding UTF8"

echo [3/4] MSBuild Release...
set "MSBUILD="
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
if "!MSBUILD!"=="" (
  echo [ERROR] MSBuild not found
  exit /b 1
)

"%MSBUILD%" "%ROOT%proxy-uid.sln" /t:Rebuild /p:Configuration=Release /v:minimal /nologo
if errorlevel 1 (
  echo [WARN] Release failed, trying Debug...
  "%MSBUILD%" "%ROOT%proxy-uid.sln" /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
  if errorlevel 1 exit /b 1
  set "BUILT=%ROOT%bin\Debug\!EXE_NAME!"
) else (
  set "BUILT=%ROOT%bin\Release\!EXE_NAME!"
)

if not exist "!BUILT!" (
  for %%F in ("%ROOT%bin\Release\*.exe") do set "BUILT=%%~fF" & goto :found
  for %%F in ("%ROOT%bin\Debug\*.exe") do set "BUILT=%%~fF" & goto :found
)

:found
if not exist "!BUILT!" (
  echo [ERROR] bin\Release ya bin\Debug me EXE nahi mila
  exit /b 1
)

echo [4/4] EXE ready — bin folder
echo [OK] !BUILT!
exit /b 0
