@echo off
echo Creating update zip...
echo Say Yes to the prompt
del Bin\*
echo Copying files...
xcopy OpenTerraria\bin\Debug Bin
xcopy OpenTerraria\bin\Debug\images Bin\images
echo Cd-ing to Bin...
cd Bin
echo Zipping it up...
..\7za a updatezip.zip *
echo Cd-ing up one level...
cd ..
echo Copying zip to this directory
del updatezip.zip
copy Bin\updatezip.zip .
echo Git stuff...
git add -A
git commit -m %1
git push -u origin master