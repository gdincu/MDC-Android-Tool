@echo off
::Model
for /f "tokens=4 delims= " %%a in ('adb devices -l ^| find "model:"') do set "model=%%a"
::Android version
for /f "tokens=1 delims=." %%a in ('adb shell getprop ro.build.version.release') do set "androidVer=%%a"
echo %model:~6%_Android_%androidVer%