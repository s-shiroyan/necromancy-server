REM https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x
SET /p VERSION=<necromancy.version
SET RUNTIMES=win-x86 win-x64 linux-x64 osx-x64
SET ZIP="C:\Program Files\7-Zip\7z.exe"
mkdir .\release
(for %%x in (%RUNTIMES%) do ( 
REM Clean
if exist .\publish\%%x-%VERSION%\ RMDIR /S /Q .\publish\%%x-%VERSION%\
REM Server
dotnet publish Necromancy.Cli\Necromancy.Cli.csproj /p:Version=%VERSION% --runtime %%x --configuration Release --output ./publish/%%x-%VERSION%/Server
REM ReleaseFiles
xcopy .\ReleaseFiles .\publish\%%x-%VERSION%\
REM PACK
if exist %ZIP% %ZIP% -ttar a dummy .\publish\%%x-%VERSION%\* -so | %ZIP% -si -tgzip a .\release\%%x-%VERSION%.tar.gz
))

REM dotnet publish Necromancy.Cli\Necromancy.Cli.csproj /p:Version=1.00 --runtime win-x64 --configuration Release --output ./publish/win-x64-1.00/Server