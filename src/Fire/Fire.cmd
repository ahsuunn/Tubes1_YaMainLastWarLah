@echo off
setlocal enabledelayedexpansion

rmdir /s /q bin obj
dotnet build --configuration Release
if %ERRORLEVEL% neq 0 (
    echo Build failed. Exiting.
    exit /b %ERRORLEVEL%
)

dotnet run --configuration Release --no-build
