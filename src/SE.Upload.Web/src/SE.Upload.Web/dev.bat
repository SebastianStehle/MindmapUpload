@echo off

echo Initial Build

call gulp scripts:rebuild
call gulp styles:rebuild

echo Watch for Changes
:start
call gulp dev
goto start