echo off

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

call "%nuget%" restore UXI.GazeToolkit.sln -NonInteractive

REM Build
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" UXI.GazeToolkit.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build

for %%p in (
	UXI.GazeToolkit
) do (
	call "%nuget%" pack ".nuget\%%p.nuspec" -symbols -o Build -p Configuration=%config% 
)
