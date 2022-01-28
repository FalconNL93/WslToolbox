Param(
    [Parameter(Mandatory = $false)]
    [ValidateSet('app', 'setup', 'archive', 'all')]
    [String]$Component,

    [Parameter(Mandatory = $false)]
    [ValidateSet('debug', 'release')]
    [String]$Environment,

    [Parameter(Mandatory = $false)]
    [ValidateSet('minimal', 'normal')]
    [String]$LogLevel,

    [Parameter(Mandatory = $false)]
    [String]$OutputPath,

    [Parameter(Mandatory = $false)]
    [String]$Version
)

if($Component -eq "")
{
    $Component = 'app'
}

if(-Not ($Environment -eq ""))
{
    $Env:ProductEnvironment = $Environment;
}

if(-Not ($LogLevel -eq ""))
{
    $BuildLogLevel = $LogLevel;
}

if(-Not ($OutputPath -eq ""))
{
    $BuildOutputPath = $OutputPath;
}

if(-Not ($Version -eq ""))
{
    $Env:ProductVersion = $Version;
}

# Default Build Variables
$BuildLogLevel = "minimal"
$BuildOutputPath = "${PSScriptRoot}\Build\${Env:TargetFramework}"
$BuildFilesFolder = "${BuildOutputPath}\bin"

# Environment Variables
$FolderNS = "WslToolbox"
$Env:ProductName = "WSL Toolbox"
$Env:ProductVersion = "0.5.999.9"
$Env:ProductDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$Env:ProductEnvironment = "Debug"
$Env:ProductUuid = "FalconNL93.WSLToolbox"
$Env:TargetFramework = "net5.0-windows10.0.19041.0"

# Setup Variables
$Env:SetupOutputDirectory = "${BuildOutputPath}"
$Env:SetupOutputFile = "${FolderNS}-${Env:ProductVersion}"

# Dotnet parameters
$ProjectFile = "${PSScriptRoot}\WslToolbox.sln"
$BuildParams = @(
    "/T:Rebuild",
    "/nologo",
    "/nr:false",
    "/p:platform=`"Any CPU`"",
    "/p:OutputPath=`"${BuildFilesFolder}`"",
    "/p:configuration=`"${Env:ProductEnvironment}`"",
    "/p:VisualStudioVersion=`"17.0`"", 
    "-v ${BuildLogLevel}"
) -join " ";

function BuildApp()
{
    dotnet build ${ProjectFile} `"${BuildParams}`"
    Write-Output "CMD: dotnet build ${ProjectFile}`r`rParams: ${BuildParams}"
    Write-Output "Builded ${Env:ProductName} version ${Env:ProductVersion}"
}

function BuildSetup()
{
    Invoke-Expression "& `".\WslToolbox.Setup\build.ps1`" ${BuildFilesFolder}"
}

function BuildArchive()
{
    $ArchiveFile = "${BuildOutputPath}\${FolderNS}-${Env:ProductVersion}.zip"

    $compress = @{
        Path = "${BuildFilesFolder}\*"
        CompressionLevel = "Optimal"
        DestinationPath = $ArchiveFile
    }

    if ((test-Path $ArchiveFile))
    {
        Write-Host "Removing existing archive ${ArchiveFile}"
        Remove-Item ${BuildOutputPath}\${FolderNS}-${Env:ProductVersion}.zip
    }

    Compress-Archive @compress
}

switch ($Component)
{
    "app" {
        BuildApp
    }

    "setup" {
        BuildSetup
    }

    "archive" {
        BuildArchive
    }

    "all" {
        BuildApp
        BuildSetup
        BuildArchive
    }
}