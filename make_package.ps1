# Run this script inside VS developer powershell

$solutionDir = (get-location).path

dotnet build -c Release

cd "AutoUnzipper\bin\Release\net6.0-windows10.0.19041.0\win10-x64"
makeappx pack /v /d "." /p $solutionDir/AutoUnzipper.msix

cd $solutionDir

SignTool sign /fd SHA256 /a /f "AutoUnzipper/AutoUnzipper_TemporaryKey.pfx" "AutoUnzipper.msix"