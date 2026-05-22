@echo off
setlocal EnableExtensions EnableDelayedExpansion
chcp 65001 >nul 2>&1
title TRYHARD — Build, Run, GitHub Push, Badges

:: Project root = parent of scripts\
set "ROOT=%~dp0.."
cd /d "%ROOT%"
if not exist "proxy-uid.sln" (
    echo [ERROR] proxy-uid.sln not found in: %ROOT%
    pause
    exit /b 1
)

echo.
echo  ╔══════════════════════════════════════════════════════════╗
echo  ║  TRYHARD UID BYPASS — Desktop Deploy + GitHub Badges     ║
echo  ╚══════════════════════════════════════════════════════════╝
echo.

:: --- GitHub repo (fixed for this project) ---
set "GITHUB_USER=studywithsunny17744-svg"
set "REPO=uidd-by-src-mani-272"
set "BRANCH=main"
set "REMOTE=https://github.com/studywithsunny17744-svg/uidd-by-src-mani-272.git"
set "WEB=https://github.com/studywithsunny17744-svg/uidd-by-src-mani-272"
echo [INFO] Target repo: !WEB!

echo.
echo [1/6] Badge config already points to !WEB!

echo [2/6] Building Release...
set "MSBUILD="
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
if "!MSBUILD!"=="" (
    echo [ERROR] MSBuild not found. Install Visual Studio 2022 Build Tools.
    pause
    exit /b 1
)
"%MSBUILD%" "%ROOT%proxy-uid.sln" /t:Rebuild /p:Configuration=Release /v:minimal /nologo
if errorlevel 1 (
    echo [WARN] Release failed — trying Debug...
    "%MSBUILD%" "%ROOT%proxy-uid.sln" /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
    if errorlevel 1 (
        echo [ERROR] Build failed.
        pause
        exit /b 1
    )
    set "EXE=%ROOT%bin\Debug\TRYHARD UID BYPASS.exe"
) else (
    set "EXE=%ROOT%bin\Release\TRYHARD UID BYPASS.exe"
)

echo [3/6] Starting app from Desktop folder...
if exist "!EXE!" (
    start "" "!EXE!"
    echo [OK] App launched: !EXE!
) else (
    echo [WARN] EXE not found — build output missing.
)

echo [4/6] Git init + chunk commits (tukde)...
where git >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Git not installed. Install from https://git-scm.com
    pause
    exit /b 1
)

if not exist "%ROOT%.git" git -C "%ROOT%" init

git -C "%ROOT%" config user.email >nul 2>&1
if errorlevel 1 (
    git -C "%ROOT%" config user.email "tryhard@local.dev"
    git -C "%ROOT%" config user.name "TRYHARD UID Bypass"
)

git -C "%ROOT%" add .gitignore .gitattributes App.config packages.config FodyWeavers.xml FodyWeavers.xsd proxy-uid.sln proxy-uid.csproj 2>nul
git -C "%ROOT%" add Core\ Properties\ utils\ Program.cs AppRunner.cs CmdShell.cs 2>nul
git -C "%ROOT%" diff --cached --quiet
if not errorlevel 1 (
    git -C "%ROOT%" commit -m "feat: core engine, CMD shell, network proxy"
) else (
    echo [INFO] Core chunk already committed or empty.
)

git -C "%ROOT%" add ui\ 2>nul
git -C "%ROOT%" diff --cached --quiet
if not errorlevel 1 (
    git -C "%ROOT%" commit -m "feat: GUI launcher and main panel"
)

git -C "%ROOT%" add .github\ README.md LICENSE scripts\ 2>nul
git -C "%ROOT%" diff --cached --quiet
if not errorlevel 1 (
    git -C "%ROOT%" commit -m "ci: GitHub Actions workflow and live badges"
)

git -C "%ROOT%" branch -M %BRANCH% 2>nul

echo [5/6] Push to GitHub...
git -C "%ROOT%" remote get-url origin >nul 2>&1
if errorlevel 1 git -C "%ROOT%" remote add origin "!REMOTE!"

where gh >nul 2>&1
if not errorlevel 1 (
    gh repo view "!GITHUB_USER!/!REPO!" >nul 2>&1
    if errorlevel 1 (
        echo [INFO] Creating repo on GitHub via gh...
        gh repo create "!REPO!" --public --source "%ROOT%" --remote origin --push 2>nul
    )
)

git -C "%ROOT%" push -u origin %BRANCH%
if errorlevel 1 (
    echo.
    echo [WARN] Push failed. Do this once manually:
    echo   1. Create repo: !WEB!
    echo   2. Then run this BAT again.
    echo.
    goto :open_badges
)

echo [OK] Pushed to !REMOTE!

:open_badges
echo [6/6] Opening GitHub — badges will go live after CI runs ~1 min...
start "" "!WEB!"
start "" "!WEB!/actions"
start "" "https://img.shields.io/github/actions/workflow/status/!GITHUB_USER!/!REPO!/build.yml?branch=%BRANCH%"

echo.
echo  Done. README badges update when Actions build is green.
echo  Repo: !WEB!
echo.
pause
endlocal
