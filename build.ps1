Param(
    [Parameter(Mandatory = $false)]
    [ValidateSet('app', 'setup', 'archive', 'info', 'all')]
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
    [String]$Version,

    [Parameter(Mandatory = $false)]
    [Switch]$Clean,

    [Parameter(Mandatory = $false)]
    [Switch]$OpenFolder
)

# Read Environment file
if(Test-Path ${PSScriptRoot}\build-env.ps1)
{
    Invoke-Expression "& ${PSScriptRoot}\build-env.ps1"
}

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

# Environment variables
$Env:ProductId = $Env:ProductId ? $Env:ProductId : "AppId"
$Env:ProductAuthor = $Env:ProductAuthor ? $Env:ProductAuthor : "Author"
$Env:ProductName = $Env:ProductName ? $Env:ProductName : "App Title"
$Env:ProductVersion = $Env:ProductVersion ? $Env:ProductVersion : "1.0.0.0"
$Env:ProductDescription = $Env:ProductDescription ? $Env:ProductDescription : "No description"
$Env:ProductEnvironment = $Env:ProductEnvironment ? $Env:ProductEnvironment : "Debug"
$Env:ProductUuid = $Env:ProductUuid ? $Env:ProductUuid : "${Env:ProductAuthor}.${Env:ProductId}"
$Env:ProductTargetFramework = $Env:ProductTargetFramework ? $Env:ProductTargetFramework : "net6.0"
$Env:ProductPlatform = $Env:ProductPlatform ? $Env:ProductPlatform : "Any CPU"

# Default build variables
$BuildLogLevel = "minimal"
$BuildOutputPath = "${PSScriptRoot}\Build\${Env:ProductEnvironment}\${Env:ProductTargetFramework}"
$BuildPackageOutputPath = "${BuildOutputPath}\output"
$BuildFilesPath = "${BuildOutputPath}\bin"

# Setup specific variables
$Env:SetupOutputPath = "${BuildPackageOutputPath}"
$Env:SetupOutputFile = "${Env:ProductId}-${Env:ProductVersion}"

# Dotnet parameters
$ProjectFile = "${PSScriptRoot}\WslToolbox.sln"
$BuildParams = @(
    "/T:Rebuild",
    "/nologo",
    "/nr:false",
    "/p:platform=`"${Env:Platform}`"",
    "/p:OutputPath=`"${BuildFilesPath}`"",
    "/p:configuration=`"${Env:ProductEnvironment}`"",
    "/p:VisualStudioVersion=`"17.0`"", 
    "-v ${BuildLogLevel}"
) -join " ";

function BuildApp()
{
    if($Clean)
    {
        dotnet clean ${ProjectFile}
        dotnet nuget locals all --clear
    }


    dotnet build ${ProjectFile} `"${BuildParams}`"
    Write-Output "CMD: dotnet build ${ProjectFile}`r`rParams: ${BuildParams}"
    Write-Output "Builded ${Env:ProductName} version ${Env:ProductVersion}"
}

function BuildSetup()
{
    Invoke-Expression "& `".\WslToolbox.Setup\build.ps1`" ${BuildFilesPath}"
}

function BuildArchive()
{
    $ArchiveFile = "${BuildPackageOutputPath}\${Env:ProductId}-${Env:ProductVersion}.zip"

    $compress = @{
        Path = "${BuildFilesPath}\*"
        CompressionLevel = "Optimal"
        DestinationPath = $ArchiveFile
    }

    if ((test-Path $ArchiveFile))
    {
        Write-Host "Removing existing archive ${ArchiveFile}"
        Remove-Item ${BuildOutputPath}\${Env:ProductId}-${Env:ProductVersion}.zip
    }

    Compress-Archive @compress
}

function OpenFolder()
{
    if(-Not($OpenFolder)) { return }

    Write-Host "Opening ${BuildOutputPath}"
    explorer "${BuildOutputPath}"
}

function ShowInfo()
{
    Write-Host "ProductId:                  $Env:ProductId"
    Write-Host "ProductAuthor:              $Env:ProductAuthor"
    Write-Host "ProductName:                $Env:ProductName"
    Write-Host "ProductVersion:             $Env:ProductVersion"
    Write-Host "ProductDescription:         $Env:ProductDescription"
    Write-Host "ProductEnvironment:         $Env:ProductEnvironment"
    Write-Host "ProductUuid:                $Env:ProductUuid"
    Write-Host "TargetFramework:            $Env:ProductTargetFramework"
    Write-Host "Platform:                   $Env:ProductPlatform"
    Write-Host "BuildLogLevel:              $BuildLogLevel"
    Write-Host "BuildPackageOutputPath:     $BuildPackageOutputPath"
    Write-Host "BuildFilesPath:             $BuildFilesPath"
}

switch ($Component)
{
    'info' {
        ShowInfo
    }

    "app" {
        BuildApp
        OpenFolder
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