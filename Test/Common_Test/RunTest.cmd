@echo off
if exist "C:\Users\KeithF\Documents\CommandPrompt\CMDPrompt.cmd" call "C:\Users\KeithF\Documents\CommandPrompt\CMDPrompt.cmd"
cd C:\Users\KeithF\Source\Repos\Common\Test\Common_Test\bin\Debug
"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\MSTest.exe" /testcontainer:Common_Test.dll /noresults /detail:errormessage
pause