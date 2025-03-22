@echo off
setlocal enabledelayedexpansion

rmdir /s /q bin obj
dotnet nuget locals all --clear
dotnet restore
dotnet build --configuration Release
if %ERRORLEVEL% neq 0 (
    echo Build failed. Exiting.
    exit /b %ERRORLEVEL%
)

for /r %%F in (bin\Release\*.nupkg) do set NUPKG=%%F
if not defined NUPKG (
    echo No .nupkg file found. Exiting.
    exit /b 1
)

for /d %%D in ("..\*Bot") do (
    set DIRNAME=%%~nxD
    if /i "!DIRNAME:~-3!"=="Bot" (
        rmdir /s /q "%%D\packages"
        xcopy /y "%NUPKG%" "%%D\packages\"
        echo Copied %NUPKG% to %%D\packages\
    )
)

echo Done.
