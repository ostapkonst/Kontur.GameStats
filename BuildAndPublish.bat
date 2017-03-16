set location="Kontur.GameStats\src\Kontur.GameStats.Server"
set packages="..\..\packages"
rem .NET Core не понимает относительные пути, баг: https://github.com/dotnet/cli/issues/3833
set output="%~dp0%Release"

@echo off
cls

cd %location%
dotnet restore --packages %packages%
dotnet publish --configuration Release --runtime win7-x86 --output %output%
pause