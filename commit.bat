@echo off
echo Running build...
start build.bat
echo Git stuff...
git add -A
git commit -m %1
git push -u origin master