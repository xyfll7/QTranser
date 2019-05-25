@ echo off
%1 %2
ver|find "5.">nul&&goto :Admin
mshta vbscript:createobject("shell.application").shellexecute("%~s0","goto :Admin","","runas",1)(window.close)&goto :eof
:Admin

rem 以上请求管理员权限

title 安装 QTranser
@setlocal enableextensions

rem Check permissions
net session >nul 2>&1
if %errorLevel% == 0 (
    echo 管理权限已确认 >nul 2>&1
) else (
    echo 安装无法继续，请以管理员权限运行此脚本
	pause
    goto EXIT
)

"%~dp0RegAsm.exe" /nologo /unregister "%~dp0..\QTranser.dll" >nul 2>&1

taskkill /f /im "explorer.exe" >nul 2>&1
start explorer.exe >nul 2>&1

rem @echo off

rem "%~dp0tools\gacutil.exe" /nologo /u "%~dp0WindowsDeskBand.dll"
rem "%~dp0tools\gacutil.exe" /nologo /u "%~dp0WPFBand.dll"
rem "%~dp0tools\gacutil.exe" /nologo /u "%~dp0BandTest.dll"
rem "%~dp0tools\RegAsm.exe" /nologo /unregister "%~dp0BandTest.dll"