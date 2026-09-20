@echo off
setlocal enabledelayedexpansion

REM ============================================================
REM
REM
REM  Vibe coded sorry :pray:.
REM
REM
REM  Usage:
REM    compress_chmd.bat "C:\path\to\ChapterMD.Cli.exe" 1.2.3
REM    compress_chmd.bat "C:\path\to\folder" 1.2.3
REM    compress_chmd.bat 1.2.3                  (searches current folder)
REM ============================================================

set "INPUT=%~1"
set "VERSION=%~2"

REM --- If only one arg given, decide if it's a version or a path ---
if "%VERSION%"=="" (
    if not "%INPUT%"=="" (
        REM If the single arg has a path separator or ends with .exe -> treat as path
        echo %INPUT% | findstr /i /c:":\" /c:"/" /c:".exe" >nul
        if not errorlevel 1 (
            set "VERSION="
        ) else (
            set "VERSION=%INPUT%"
            set "INPUT="
        )
    )
)

REM --- If no input path given, default to current folder ---
if "%INPUT%"=="" set "INPUT=ChapterMD.Cli.exe"

REM --- Resolve input: if it's a folder, append the exe name ---
if exist "%INPUT%\" (
    set "INPUT=%INPUT%\ChapterMD.Cli.exe"
)

REM --- Verify the target exe exists ---
if not exist "%INPUT%" (
    echo [ERROR] Target executable not found:
    echo         %INPUT%
    echo.
    echo Usage: %~nx0 "path\to\ChapterMD.Cli.exe" ^<version^>
    echo        %~nx0 "path\to\folder"           ^<version^>
    echo        %~nx0 ^<version^>                 ^(uses current folder^)
    pause
    exit /b 1
)

REM --- Make absolute path and split into dir + filename ---
for %%F in ("%INPUT%") do (
    set "TARGET_FULL=%%~fF"
    set "TARGET_NAME=%%~nxF"
    set "TARGET_DIR=%%~dpF"
)
echo Target: %TARGET_FULL%

REM --- Locate 7-Zip ---
set "SEVENZIP="
for %%P in (
    "%ProgramFiles%\7-Zip\7z.exe"
    "%ProgramFiles(x86)%\7-Zip\7z.exe"
    "%ProgramW6432%\7-Zip\7z.exe"
) do (
    if exist %%P if not defined SEVENZIP set "SEVENZIP=%%~P"
)
if not defined SEVENZIP (
    where 7z.exe >nul 2>&1 && set "SEVENZIP=7z.exe"
)
if not defined SEVENZIP (
    echo [ERROR] 7-Zip not found. Install it or add 7z.exe to PATH.
    pause
    exit /b 1
)
echo Using 7-Zip: %SEVENZIP%

REM --- Prompt for version if still missing ---
if "%VERSION%"=="" (
    set /p "VERSION=Enter version number (e.g. 1.0.0): "
)
if "%VERSION%"=="" (
    echo [ERROR] No version provided.
    pause
    exit /b 1
)

set "OUTDIR=../Archives"
for %%I in ("%OUTDIR%") do set "OUTDIR=%%~fI"
if not exist "%OUTDIR%" mkdir "%OUTDIR%"
set "ARCHIVE=%OUTDIR%\CHMD.Cli-v%VERSION%-x64.7z"

REM --- Warn if archive already exists ---
if exist "%ARCHIVE%" (
    echo [WARN] Archive already exists and will be overwritten:
    echo        %ARCHIVE%
)
REM --- Create the archive (target exe goes into it, so cd to its folder) ---
echo.
echo Compressing "%TARGET_NAME%" -^> "%ARCHIVE%" ...
pushd "%TARGET_DIR%"
"%SEVENZIP%" a -t7z -m0=lzma2 -mx=9 -mfb=273 -md=256m -ms=on -mmt=on "%ARCHIVE%" "%TARGET_NAME%"
set "RC=%ERRORLEVEL%"
popd

if not "%RC%"=="0" (
    echo.
    echo [ERROR] Compression failed.
    pause
    exit /b 1
)

echo.
echo [OK] Created: %ARCHIVE%
echo.
endlocal
pause