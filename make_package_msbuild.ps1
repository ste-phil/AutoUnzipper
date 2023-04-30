$Solution_Name="AutoUnzipper.sln"
$Appx_Bundle="Never"
$Appx_Bundle_Platforms="x64"
$Appx_Package_Build_Mode="SideloadOnly"
$Appx_Package_Dir="Packages\"
$Configuration="Release"

#restore
msbuild $Solution_Name /t:Restore /p:Configuration=$Configuration /p:Platform=$Appx_Bundle_Platforms
# msbuild $Solution_Name /t:Restore /p:RuntimeIdentifiers=win10-x64

#build
msbuild $Solution_Name `
    /p:AppxBundlePlatforms="$Appx_Bundle_Platforms"  `
    /p:Configuration=$Configuration `
    /p:UapAppxPackageBuildMode=$Appx_Package_Build_Mode  `
    /p:Platform=$Appx_Bundle_Platforms `
    /p:AppxBundle=$Appx_Bundle  `
    /p:PackageCertificateKeyFile=AutoUnzipper_TemporaryKey.pfx  `
    /p:PublishSingleFile=false `
    /P:PublishReadyToRun=true `
    /p:SelfContained=true `
    /p:EnableMsixTooling=true `
    /p:WindowsAppSDKSelfContained=truz `
    /p:AppxPackageDir=$Appx_Package_Dir `
    /p:GenerateAppxPackageOnBuild=true
