@echo off
for /f "tokens=6 delims= " %%a in ('adb shell dumpsys activity recents ^| find "Recent #0"') do set "model=%%a"
set model=%model:~2%
adb shell am force-stop %model%

