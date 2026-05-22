@echo off
setlocal EnableExtensions EnableDelayedExpansion

cd /d "%~dp0.."
set "ROOT=%CD%\"
set "SLN=%ROOT%proxy-uid.sln"

if not exist "%SLN%" (
    echo [ERROR] proxy-uid.sln not found: %SLN%
    pause
    exit /b 1
)

title MANI 272 - GitHub Push

echo.
echo ============================================================
echo   MANI 272 UID BYPASS - Build + Run + GitHub Push
echo ============================================================
echo.

echo [0/7] Apply branding.config ...
call "%~dp0Apply-Branding.bat"
if errorlevel 1 (
    echo [ERROR] branding.config apply failed.
    pause
    exit /b 1
)

set "GITHUB_USER=studywithsunny17744-svg"
set "REPO_NAME=uidd-by-src-mani-272"
set "GIT_BRANCH=main"
set "GIT_REMOTE=https://github.com/studywithsunny17744-svg/uidd-by-src-mani-272.git"
set "GIT_WEB=https://github.com/studywithsunny17744-svg/uidd-by-src-mani-272"

echo [1/7] Repo: !GIT_WEB!
echo.

echo [2/7] Building ...
set "MSBUILD="
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
)
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    set "MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
)
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set "MSBUILD=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)
if "!MSBUILD!"=="" (
    echo [ERROR] MSBuild not found.
    pause
    exit /b 1
)

"!MSBUILD!" "!SLN!" /t:Rebuild /p:Configuration=Release /v:minimal /nologo
if errorlevel 1 (
    echo [WARN] Release failed, trying Debug ...
    "!MSBUILD!" "!SLN!" /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
    if errorlevel 1 (
        echo [ERROR] Build failed.
        echo Path: !SLN!
        pause
        exit /b 1
    )
    set "OUT_DIR=!ROOT!bin\Debug\"
) else (
    set "OUT_DIR=!ROOT!bin\Release\"
)

set "EXE_PATH="
for %%F in ("!OUT_DIR!*.exe") do set "EXE_PATH=%%~fF"

echo [3/7] Run app ...
if exist "!EXE_PATH!" (
    start "" "!EXE_PATH!"
    echo [OK] Started: !EXE_PATH!
) else (
    echo [WARN] EXE not found in !OUT_DIR!
)

echo [4/7] Git save changes ...
where git >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Git not installed.
    pause
    exit /b 1
)

pushd "%ROOT%"
if not exist ".git" git init
git config user.email >nul 2>&1
if errorlevel 1 (
    git config user.email "mani272@local.dev"
    git config user.name "MANI 272 UID Bypass"
)

git branch -M !GIT_BRANCH! 2>nul
git remote get-url origin >nul 2>&1
if errorlevel 1 git remote add origin "!GIT_REMOTE!"

git add -A
git diff --cached --quiet
if not errorlevel 1 (
    git commit -m "update: branding config and project"
    echo [OK] Local commit created.
) else (
    echo [INFO] No new files to commit.
)

echo [5/7] Sync remote then push ...
git fetch origin !GIT_BRANCH! 2>nul
if not errorlevel 1 (
    git rev-parse origin/!GIT_BRANCH! >nul 2>&1
    if not errorlevel 1 (
        echo [INFO] GitHub par naye commits hain - pull kar rahe hain ...
        git pull --rebase origin !GIT_BRANCH!
        if errorlevel 1 (
            echo [WARN] Rebase fail - merge try ...
            git pull origin !GIT_BRANCH! --no-edit
            if errorlevel 1 (
                echo [ERROR] Pull failed. Conflict fix karo phir dubara BAT chalao.
                popd
                pause
                exit /b 1
            )
        )
        echo [OK] Remote changes merged.
    )
)

git push -u origin !GIT_BRANCH!
set PUSH_ERR=!errorlevel!
popd

if !PUSH_ERR! neq 0 (
    echo [WARN] Push failed. GitHub login / token check karo.
    echo Repo: !GIT_WEB!
    goto OPEN_URLS
)

echo [OK] Push successful - code GitHub par hai.

:OPEN_URLS
echo [6/7] Open GitHub ...
start "" "!GIT_WEB!"
start "" "!GIT_WEB!/actions"

echo.
echo Done. Repo: !GIT_WEB!
echo.
pause
endlocal

