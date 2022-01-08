if (!${env:APPVEYOR_BUILD_VERSION}) { $env:APPVEYOR_BUILD_VERSION = "1.0.0" }
if (!${env:APPVEYOR_REPO_BRANCH}) { $env:APPVEYOR_REPO_BRANCH = "develop" }
if (!${env:PRODUCT_ENVIRONMENT}) { $env:PRODUCT_ENVIRONMENT = "Debug" }
if (!${env:PRODUCT_UUID}) { 
    Write-Output "No UUID, using default."
    $env:PRODUCT_UUID = "4b65eb98-5364-4817-bc03-b67a46b0d991"
}

$NsisFile = "wsltoolbox.nsi"
$ProductEnvironment = "${env:PRODUCT_ENVIRONMENT}"
$ProductName = "WSL Toolbox"
$ProductVersion = "${env:APPVEYOR_BUILD_VERSION}.0"
$ExecutableName = "wsltoolbox-${env:APPVEYOR_REPO_BRANCH}-v${env:APPVEYOR_BUILD_VERSION}-setup"
$TargetDirectory = "net5.0-windows10.0.19041.0"

C:\Program' Files (x86)'\NSIS\makensis.exe `
    /DEXECUTABLE_NAME="${ExecutableName}" `
    /DPRODUCT_VERSION="${ProductVersion}" `
    /DPRODUCT_ENVIRONMENT="${ProductEnvironment}" `
    /DPRODUCT_NAME="${ProductName}" `
    /DTARGET_DIRECTORY="${TargetDirectory}" `
    /DDISPLAY_VERSION="${env:APPVEYOR_BUILD_VERSION}" `
    /DUUID="${env:PRODUCT_UUID}" `
    ${PSScriptRoot}\${NsisFile}

Remove-Item Env:\APPVEYOR_BUILD_VERSION
Remove-Item Env:\APPVEYOR_REPO_BRANCH
Remove-Item Env:\PRODUCT_ENVIRONMENT
Remove-Item Env:\PRODUCT_UUID