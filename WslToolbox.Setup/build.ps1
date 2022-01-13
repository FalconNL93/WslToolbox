Param(
    [Parameter(Mandatory=$false)]
    [Switch]$Run,

    [Parameter(Mandatory=$false)]
    [Switch]$Silent,

    [Parameter(Mandatory=$false)]
    [Switch]$Info
)

$Compiler = ${env:INNO_COMPILER_PATH} ? ${env:INNO_COMPILER_PATH} : "${env:ProgramFiles(x86)}\Inno Setup 6\iscc.exe"
$InnoFile = "${PSScriptRoot}\wsltoolbox-inno.iss"

$ProductName = "WSL Toolbox"
$ProductDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$OutputDirectory = "${PSScriptRoot}\bin"
$ProductUuid = ${env:PRODUCT_UUID} ? ${env:PRODUCT_UUID} : "4b65eb98-5364-4817-bc03-b67a46b0d991"
$ProductVersion = ${env:APPVEYOR_BUILD_VERSION} ? ${env:APPVEYOR_BUILD_VERSION} : "0.5.0"
$ProductBranch = $env:APPVEYOR_REPO_BRANCH ? $env:APPVEYOR_REPO_BRANCH : "develop"
$ProductEnvironment = $env:PRODUCT_ENVIRONMENT ? $env:PRODUCT_ENVIRONMENT : "Debug"
$ExecutableName = "wsltoolbox-${ProductBranch}-v${ProductVersion}-setup"

if (!(test-Path $Compiler)) {
    Write-Output "${Compiler} not found."
    exit 1
}

if (!(test-Path ${InnoFile})) {
    Write-Output "${InnoFile} not found."
    exit 1
}

if (!(test-Path $OutputDirectory)) {
    New-Item "$OutputDirectory" -itemType Directory
}

"{0} {1} - {2}`r`n{3}`r`n{4}`r`n{5}" -f `
    ${ProductName}, ${ProductVersion}, ${ProductEnvironment}, `
    ${ProductUuid}, `
    "${OutputDirectory}\${ExecutableName}.exe", `
    $Compiler

if($Info) { exit 0; }

& ${Compiler} `
    /DProductName="${ProductName}" `
    /DProductVersion="${ProductVersion}" `
    /DProductDescription="${ProductDescription}" `
    /DProductEnvironment="${ProductEnvironment}" `
    /DProductUuid="${ProductUuid}" `
    /DOutputFile="${ExecutableName}" `
    /DOutputDirectory="${OutputDirectory}" `
    ${InnoFile}

if(!($?)) { Write-Output "Build failed."; exit $?; }

if($Run) {
    $SetupFile = "${OutputDirectory}\${ExecutableName}.exe"
    $Parameters;

    if (!(test-Path $SetupFile)) {
        Write-Output "${SetupFile} not found."
        exit 1
    }

    if($Silent) {
        $Parameters = $Parameters + "/SILENT"
    }

    Write-Output "Launching ${SetupFile} ${Parameters}"
    & ${SetupFile} ${Parameters}
}