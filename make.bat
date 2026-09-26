@echo off

FOR /F "tokens=* USEBACKQ" %%F IN (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) DO (
SET msbuild="%%F"
)
ECHO %msbuild%
if not defined msbuild (
  echo MSBuild not found
  goto error
)

@%msbuild% authenticator.sln /t:restore /p:RestorePackagesConfig=true
if errorlevel 1 goto error
@%msbuild% authenticator.sln /t:Rebuild /p:DebugType=None /p:Configuration=Release

if errorlevel 1 goto error

@%msbuild% TwoFactorAuth.sln /t:restore /p:RestorePackagesConfig=true
if errorlevel 1 goto error
@%msbuild% TwoFactorAuth.sln /t:Rebuild /p:DebugType=None /p:Configuration=Release

if errorlevel 1 goto error

goto exit
:error
pause
:exit
