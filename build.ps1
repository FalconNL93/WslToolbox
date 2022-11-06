# For local building
$ErrorActionPreference = "Stop"

$env:GITHUB_WORKSPACE = $PSScriptRoot;
$env:Solution_Name = "WslToolbox.sln";
$env:Configuration = "Release";
$env:ProductOwner = 'FalconNL93';
$env:ProductExecutable = 'toolbox.exe';
$env:ProductName = 'WSL Toolbox';
$env:ProductDescription = 'WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface.';
$env:ProductUuid = 'FalconNL93.WSLToolbox';
$env:ProductVersion = '0.6';
$env:ProductEnvironment = 'dev';
$env:ProductUrl = 'https://github.com/FalconNL93/wsltoolbox';
$env:SetupOutputFile = 'wsltoolbox-0.6';

#dotnet msbuild $env:Solution_Name /t:Restore /p:Configuration=$env:Configuration
#dotnet msbuild WslToolbox.UI\WslToolbox.UI.csproj /p:Configuration=$env:Configuration /p:Platform=x64 /p:outdir="$env:GITHUB_WORKSPACE\app\release\x64"
& .\WslToolbox.Setup\build.ps1 -BinariesDirectory $env:GITHUB_WORKSPACE\app\release\x64
