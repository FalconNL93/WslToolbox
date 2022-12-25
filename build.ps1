# Parameters
Param(
    [ValidateSet(
        "build",
        "clean",
        "inno"
    )]
    [Parameter(Mandatory = $false)]
    [string]$Action = "x64",

    [Parameter(Mandatory = $false)]
    [Switch]$WithMsix,

    [Parameter(Mandatory = $false)]
    [Switch]$Inno
)

function Header {
    Write-Output "`n===================================="
}

# For local building
$ErrorActionPreference = "Stop"

$Platform = "x64";
$AppDirectory = $PSScriptRoot;
$RootProjectDirectory = "$(Get-Location)\WslToolbox.UI";
$RootProject = "$RootProjectDirectory\WslToolbox.UI.csproj";
$AppxManifest = "$RootProjectDirectory\Package.appxmanifest";
$AppUuid = 'FalconNL93.WSLToolbox';
$AppName = "WSL Toolbox"
$AppDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$AppExecutable = "toolbox.exe";
$AppVersion = '0.6.17.0';
$AppUrl = "https://github.com/FalconNL93/wsltoolbox";
$AppOwner = "FalconNL93"
$SetupOutputFile = "wsltoolbox_${AppVersion}_${Platform}_setup";

$CommandName = $PSCmdlet.MyInvocation.InvocationName;
$ParameterList = (Get-Command -Name $CommandName).Parameters;
foreach ($Parameter in $ParameterList) {
    Get-Variable -Name $Parameter.Values.Name -ErrorAction SilentlyContinue;
}
Header

function Publish {
    if($WithMsix)
    {
        [xml]$manifest= get-content "$AppxManifest";
        $manifest.Package.Identity.Version = "$AppVersion";
        $manifest.save("$AppxManifest");
    }

    dotnet publish "$RootProject" `
    --nologo `
    -p:PublishProfile="$Platform" `
    -p:Version="$AppVersion" `
    -p:FileVersion="$AppVersion" `
    -p:AssemblyVersion="$AppVersion" `
    -p:GenerateAppxPackageOnBuild=$WithMsix `
    -p:AppxPackageDir="$AppDirectory\app\release\$Platform-msix\" `
    --self-contained `
    -r win-x64 `
    -o "$AppDirectory\app\release\$Platform"
}

function CleanSolution {
    dotnet clean
    Remove-Item "$AppDirectory\app" -Recurse -Verbose -ErrorAction SilentlyContinue
    Remove-Item "$AppDirectory\WslToolbox.Core\bin","$AppDirectory\WslToolbox.Core\obj" -Recurse -Verbose -ErrorAction SilentlyContinue
    Remove-Item "$AppDirectory\WslToolbox.Msix\bin","$AppDirectory\WslToolbox.Msix\obj" -Recurse -Verbose -ErrorAction SilentlyContinue
    Remove-Item "$AppDirectory\WslToolbox.UI\bin","$AppDirectory\WslToolbox.UI\obj" -Recurse -Verbose -ErrorAction SilentlyContinue
    Remove-Item "$AppDirectory\WslToolbox.UI.Core\bin","$AppDirectory\WslToolbox.UI.Core\obj" -Recurse -Verbose -ErrorAction SilentlyContinue
}

function InnoSetup {
    .\WslToolbox.Setup\build.ps1 `
    -AppDirectory "$AppDirectory\app\release\$Platform" `
    -AppUuid "$AppUuid" `
    -AppName "$AppName" `
    -AppDescription "$AppDescription" `
    -AppExecutable "$AppExecutable" `
    -AppVersion "$AppVersion" `
    -AppUrl "$AppUrl" `
    -AppOwner "$AppOwner" `
    -SetupOutputFile "$SetupOutputFile"
}

Switch ($Action)
{
    "clean" 
    { 
        CleanSolution; 
        break; 
    }
    "inno"
    {
        InnoSetup;
        break;
    }
    default 
    { 
        Publish; 

        if($Inno)
        {
            InnoSetup;
        }
        
        Header;
        Write-Output "Output Directory: $AppDirectory\app\release\$Platform\";
        Write-Output "AppX Directory: $AppDirectory\app\release\$Platform-msix\";
        
        break;
    }
}
