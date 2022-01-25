Param(
    [Parameter(Mandatory = $true)]
    [String]$BinariesDirectory,

    [Parameter(Mandatory = $false)]
    [Switch]$Run,

    [Parameter(Mandatory = $false)]
    [Switch]$Silent,

    [Parameter(Mandatory = $false)]
    [Switch]$Info
)

$Compiler = ${env:INNO_COMPILER_PATH} ? ${env:INNO_COMPILER_PATH} : "${env:ProgramFiles(x86)}\Inno Setup 6\iscc.exe"
$InnoFile = "${PSScriptRoot}\setup.iss"
$OutputDirectory = "${PSScriptRoot}\bin"

$ProductName = ${env:ProductName}
$ProductDescription = ${env:ProductDescription}
$ProductUuid = ${env:ProductUuid}
$ProductVersion = ${env:ProductVersion}
$ProductEnvironment = ${env:ProductEnvironment}
$OutputFile = ${env:OutputFile}
$TargetFramework = ${env:ProductTargetFramework}

if (!(test-Path ${BinariesDirectory}))
{
    Write-Output "${BinariesDirectory} does not exist."
    exit 1
}

if (!(test-Path $Compiler))
{
    Write-Output "${Compiler} not found."
    exit 1
}

if (!(test-Path ${InnoFile}))
{
    Write-Output "${InnoFile} not found."
    exit 1
}

if (!(test-Path $OutputDirectory))
{
    New-Item "$OutputDirectory" -itemType Directory
}

"{0} {1} - {2}`r`n{3}`r`n{4}`r`n{5}`r`n{6}\*.*" -f `
    ${ProductName}, ${ProductVersion}, ${ProductEnvironment},    `
       ${ProductUuid},    `
       "${OutputDirectory}\${OutputFile}.exe",    `
       $Compiler,    `
       $BinariesDirectory

if ($Info)
{
    exit 0;
}

& ${Compiler} `
    /DProductName="${ProductName}" `
    /DProductVersion="${ProductVersion}" `
    /DProductDescription="${ProductDescription}" `
    /DProductEnvironment="${ProductEnvironment}" `
    /DProductUuid="${ProductUuid}" `
    /DOutputFile="${OutputFile}" `
    /DOutputDirectory="${OutputDirectory}" `
    /DTargetFramework="${TargetFramework}" `
    /DBinariesDirectory="${BinariesDirectory}" `
    ${InnoFile}

if (!($?))
{
    Write-Output "Build failed."; exit $?;
}

if ($Run)
{
    $SetupFile = "${OutputDirectory}\${OutputFile}.exe"
    $Parameters;

    if (!(test-Path $SetupFile))
    {
        Write-Output "${SetupFile} not found."
        exit 1
    }

    if ($Silent)
    {
        $Parameters = $Parameters + "/SILENT"
    }

    Write-Output "Launching ${SetupFile} ${Parameters}"
    & ${SetupFile} ${Parameters}
}