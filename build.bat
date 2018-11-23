echo off

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

REM Restore packages
call "%nuget%" restore UXI.GazeToolkit.sln -NonInteractive

REM Build
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" UXI.GazeToolkit.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build

for %%p in (
	UXI.GazeToolkit
	UXI.GazeToolkit.Serialization
) do (
	call "%nuget%" pack ".nuget\%%p.nuspec" -Symbols -OutputDirectory build -Properties Configuration=%config% 
)
