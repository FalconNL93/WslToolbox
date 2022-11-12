Param(
    [ValidateSet(
        "x86", 
        "x64"
    )]
    [Parameter(Mandatory = $true)]
    [String]$Platform,

    [Parameter(Mandatory = $false)]
    [Switch]$WithSetup
)

# For local building
$ErrorActionPreference = "Stop"

$CommandName = $PSCmdlet.MyInvocation.InvocationName;
$ParameterList = (Get-Command -Name $CommandName).Parameters;
foreach ($Parameter in $ParameterList) {
    Get-Variable -Name $Parameter.Values.Name -ErrorAction SilentlyContinue;
}

$RootProject = "WslToolbox.UI\WslToolbox.UI.csproj";
$AppDirectory = $PSScriptRoot;
$AppUuid = 'FalconNL93.WSLToolbox';
$AppName = "WSL Toolbox"
$AppDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$AppExecutable = "toolbox.exe";
$AppVersion = '0.6.0';
$AppUrl = "https://github.com/FalconNL93/wsltoolbox";
$AppOwner = "FalconNL93"
$SetupOutputFile = "wsltoolbox-0.6-$Platform";

dotnet publish "$RootProject" `
    -p:PublishProfile="$Platform" `
    -p:Version="1.0.0.0" `
    -p:FileVersion="1.0.0.0" `
    -p:AssemblyVersion="1.0.0.0" `
    -p:PublishTrimmed=True `
    -p:TrimMode=CopyUsed `
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