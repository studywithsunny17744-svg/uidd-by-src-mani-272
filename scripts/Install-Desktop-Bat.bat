@echo off

setlocal

chcp 65001 >nul 2>&1



set "SCRIPTS=%~dp0"

cd /d "%SCRIPTS%.."



echo.

echo  Installing Desktop runner from branding.config...

echo.



call "%SCRIPTS%Apply-Branding.bat"

if errorlevel 1 (

    pause

    exit /b 1

)



echo.

echo  Done. Check your Desktop for the .bat file name from branding.config.

echo.

set /p RUN=Run deploy BAT now? [Y/n]: 

if /i "%RUN%"=="n" goto :done

for %%F in ("%USERPROFILE%\Desktop\*.bat") do (

    findstr /i "GitHub-Push-Badges.bat" "%%F" >nul 2>&1 && (

        call "%%F"

        goto :done

    )

)

echo [WARN] Desktop runner not found. Run Apply-Branding.bat again.



:done

pause

endlocal

