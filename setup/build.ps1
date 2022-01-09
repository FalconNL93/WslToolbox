Param(
    [Parameter(Mandatory=$false)]
    [Switch]$Run
)

$MakeNsis = "C:\Program Files (x86)\NSIS\makensis.exe"
$NsisFile = "wsltoolbox.nsi"

if (!${env:APPVEYOR_BUILD_VERSION}) { $env:APPVEYOR_BUILD_VERSION = "1.0.0" }
if (!${env:APPVEYOR_REPO_BRANCH}) { $env:APPVEYOR_REPO_BRANCH = "develop" }
if (!${env:PRODUCT_ENVIRONMENT}) { $env:PRODUCT_ENVIRONMENT = "Debug" }
if (!${env:PRODUCT_UUID}) { 
    Write-Output "No UUID, using default."
    $env:PRODUCT_UUID = "4b65eb98-5364-4817-bc03-b67a46b0d991"
}

$ProductName = "WSL Toolbox"
$ProductDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$DialogCaption = "${ProductName} ${env:APPVEYOR_BUILD_VERSION}"
$ExecutableName = "wsltoolbox-${env:APPVEYOR_REPO_BRANCH}-v${env:APPVEYOR_BUILD_VERSION}-setup"
$ProductEnvironment = "${env:PRODUCT_ENVIRONMENT}"
$ProductVersion = "${env:APPVEYOR_BUILD_VERSION}"
$TargetDirectory = "net5.0-windows10.0.19041.0"
$Branding = "${ProductName} ${ProductVersion}"

if (!(test-Path $MakeNsis)) {
    Write-Output "${MakeNsis} not found."
    exit 1
}

& ${MakeNsis} `
    /DROOT_FOLDER="${PSScriptRoot}" `
    /DPRODUCT_NAME="${ProductName}" `
    /DPRODUCT_DESCRIPTION="${ProductDescription}" `
    /DDIALOG_CAPTION="${DialogCaption}" `
    /DEXECUTABLE_NAME="${ExecutableName}" `
    /DPRODUCT_ENVIRONMENT="${ProductEnvironment}" `
    /DPRODUCT_VERSION="${ProductVersion}" `
    /DTARGET_DIRECTORY="${TargetDirectory}" `
    /DBRANDING="${Branding}" `
    /DUUID="${env:PRODUCT_UUID}" `
    ${PSScriptRoot}\${NsisFile}

if(!($?))
{
    Write-Output "Build failed."
    exit $?
}

if($Run) {
    $SetupFile = "${PSScriptRoot}\${ExecutableName}.exe"

    if (!(test-Path $SetupFile)) {
        Write-Output "${SetupFile} not found."
        exit 1
    }

    Write-Output "Launching ${SetupFile}"
    & ${SetupFile}
}