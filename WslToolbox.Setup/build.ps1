Param(
# Required parameters
    [Parameter(Mandatory = $true)]
    [String]$AppDirectory,

    [Parameter(Mandatory = $true)]
    [String]$AppUuid,

    [Parameter(Mandatory = $true)]
    [String]$AppName,

    [Parameter(Mandatory = $true)]
    [String]$AppDescription,

    [Parameter(Mandatory = $true)]
    [String]$AppExecutable,

    [Parameter(Mandatory = $true)]
    [String]$AppVersion,

    [Parameter(Mandatory = $true)]
    [String]$AppUrl,

    [Parameter(Mandatory = $true)]
    [String]$AppOwner,

    [Parameter(Mandatory = $true)]
    [String]$SetupOutputFile,

# Additional parameters
    [Parameter(Mandatory = $false)]
    [String]$SetupOutputDirectory,

    [Parameter(Mandatory = $false)]
    [Switch]$Run,

    [Parameter(Mandatory = $false)]
    [Switch]$Silent,

    [Parameter(Mandatory = $false)]
    [Switch]$Info
)


$Compiler = ${env:INNO_COMPILER_PATH} ? ${env:INNO_COMPILER_PATH} : "${env:ProgramFiles(x86)}\Inno Setup 6\iscc.exe"
$InnoFile = "${PSScriptRoot}\setup.iss"
$OutputDirectory = ${SetupOutputDirectory} ? ${SetupOutputDirectory} : "${PSScriptRoot}\bin"

$CommandName = $PSCmdlet.MyInvocation.InvocationName;
$ParameterList = (Get-Command -Name $CommandName).Parameters;
foreach ($Parameter in $ParameterList)
{
    Get-Variable -Name $Parameter.Values.Name -ErrorAction SilentlyContinue;
}

if (!(test-Path ${AppDirectory}))
{
    Write-Output "${AppDirectory} does not exist."
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


if ($Info)
{
    exit 0;
}

& ${Compiler} `
    /DAppDirectory="${AppDirectory}" `
    /DAppUuid="${AppUuid}" `
    /DAppName="${AppName}" `
    /DAppDescription="${AppDescription}" `
    /DAppExecutable="${AppExecutable}" `
    /DAppVersion="${AppVersion}" `
    /DAppUrl="${AppUrl}" `
    /DAppOwner="${AppOwner}" `
    /DSetupOutputFile="${SetupOutputFile}" `
    /DSetupOutputDirectory="${OutputDirectory}" `
    ${InnoFile}

if (!($?))
{
    Write-Output "Build failed."; exit $?;
}

if ($Run)
{
    $SetupFile = "${OutputDirectory}\${SetupOutputFile}.exe"
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

    Write-Output "Launching ${OutputDirectory} ${Parameters}"
    & ${OutputDirectory} ${Parameters}
}