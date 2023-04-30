# Run this script inside VS developer powershell

$solutionDir = (get-location).path

cd AutoUnzipper

# dotnet build -f net6.0-windows10.0.19041.0 -c Release -r win10-x64 --self-contained
dotnet build /p:PublishProfile=win10-x64 /verbosity:d

# xcopy "Assets\" "bin\Release\net6.0-windows10.0.19041.0\win10-x64\Assets\"
cd "bin\Debug\net6.0-windows10.0.19041.0\win10-x64"

makeappx pack /v /d "." /p $solutionDir/AutoUnzipper.msix

cd $solutionDir

SignTool sign /fd SHA256 /a /f "AutoUnzipper/AutoUnzipper_TemporaryKey.pfx" "AutoUnzipper.msix"