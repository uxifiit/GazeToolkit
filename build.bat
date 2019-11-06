@echo off

REM First find the latest version of MSBuild.exe in predefined locations.
REM Result is stored in the %msbuild% variable.

for %%f in (
    "%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
    "%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"
) do ( 
    if exist %%f (
        set msbuild=%%f
        goto AfterMSBuildLookup
    )
)

:AfterMSBuildLookup

if not defined msbuild (
    echo MSBuild.exe not found
    pause
    exit /b
)


REM Set working directory (wd) to the path in the 1st argument of the script.
REM If no argument is defined, current directory of the caller (env. variable CD) is used.
set wd=%~1
if not defined wd (
    set wd=%CD%
)

set config=%~2
if not defined config (
   set config=Release
)


REM Find solution files in the current directory and 
for /F "delims=|" %%s in (
    'dir "%wd%\*.sln" /B'
) do (
    REM Restore packages
    call "%nuget%" restore "%wd%\%%s" -NonInteractive
    
    REM Build
    call %msbuild% "%wd%\%%s" /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
)


REM Package
mkdir "%wd%\Build"

set nuspecs=.nuget

for /F "delims=|" %%p in ( 
    'dir "%wd%\%nuspecs%\*.nuspec" /B'
) do ( 
    call "%nuget%" pack "%wd%\%nuspecs%\%%p" -Symbols -OutputDirectory "%wd%\Build" -Properties Configuration=%config%
)
