echo off
set DOCFX=%1
if "%DOCFX%"=="" (
  set DOCFX=docfx.exe
)
git checkout master
%DOCFX% docs\docfx.json
RMDIR /S /Q c:\temp\docs\
xcopy docs\_site c:\temp\docs\ /s /e /y
xcopy update-docs-commit.cmd c:\temp\ /y
git reset master --hard
start cmd /k call c:\temp\update-docs-commit.cmd %cd%