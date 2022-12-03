Param(
    [ValidateSet(
        "x86", 
        "x64"
    )]
    [Parameter(Mandatory = $true)]
    [String]$Platform,

    [Parameter(Mandatory = $false)]
    [Switch]$WithSetup,

    [Parameter(Mandatory = $false)]
    [Switch]$WithMsix
)

# For local building
$ErrorActionPreference = "Stop"

$CommandName = $PSCmdlet.MyInvocation.InvocationName;
$ParameterList = (Get-Command -Name $CommandName).Parameters;
foreach ($Parameter in $ParameterList) {
    Get-Variable -Name $Parameter.Values.Name -ErrorAction SilentlyContinue;
}

$RootProjectDirectory = "$(Get-Location)\WslToolbox.UI"
$RootProject = "$RootProjectDirectory\WslToolbox.UI.csproj";
$AppxManifest = "$RootProjectDirectory\Package.appxmanifest";
$AppDirectory = $PSScriptRoot;
$AppUuid = 'FalconNL93.WSLToolbox';
$AppName = "WSL Toolbox"
$AppDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$AppExecutable = "toolbox.exe";
$AppVersion = '0.6.0';
$AppUrl = "https://github.com/FalconNL93/wsltoolbox";
$AppOwner = "FalconNL93"
$SetupOutputFile = "wsltoolbox-0.6-$Platform";

if($WithMsix)
{
    [xml]$manifest= get-content "$AppxManifest"
    $manifest.Package.Identity.Version = "$AppVersion.0"
    $manifest.save("$AppxManifest")
}

dotnet publish "$RootProject" `
    -p:PublishProfile="$Platform" `
    -p:Version="$AppVersion" `
    -p:FileVersion="$AppVersion" `
    -p:AssemblyVersion="$AppVersion" `
    -p:GenerateAppxPackageOnBuild=$WithMsix `
    -p:AppxPackageDir="$AppDirectory\app\release\$Platform-msix\" `
    --self-contained `
    -r win-x64 `
    --nologo `
    -o "$AppDirectory\app\release\$Platform"
if (!$?) { exit 1; }

if (!$WithSetup) {
    exit 0;
}

& .\WslToolbox.Setup\build.ps1 `
    -AppDirectory "$AppDirectory\app\release\$Platform" `
    -AppUuid "$AppUuid" `
    -AppName "$AppName" `
    -AppDescription "$AppDescription" `
    -AppExecutable "$AppExecutable" `
    -AppVersion "$AppVersion" `
    -AppUrl "$AppUrl" `
    -AppOwner "$AppOwner" `
    -SetupOutputFile "$SetupOutputFile"

if (!$?) { exit 1; }