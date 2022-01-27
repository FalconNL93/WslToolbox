Param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet('gui', 'setup')]
    [String]$Component
)

# Environment Variables
$FolderNS = "WslToolbox"
$Env:ProductName = "WSL Toolbox"
$Env:ProductVersion = "0.5.999.9"
$Env:ProductDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$Env:ProductEnvironment = "Debug"
$Env:ProductUuid = "FalconNL93.WSLToolbox"
$Env:OutputFile = "${FolderNS}-${Env:ProductVersion}"
$Env:TargetFramework = "net5.0-windows10.0.19041.0"

# Dotnet parameters
$Dotnet = "dotnet"
$ProjectFile = "${PSScriptRoot}\WslToolbox.sln"
$BuildParam = "/T:Rebuild /nologo /nr:false /p:platform=`"Any CPU`" /p:OutputDir=`"${PSScriptRoot}\Build`" /p:configuration=`"${Env:ProductEnvironment}`" /p:VisualStudioVersion=`"17.0`""

# Call Setup Build
function BuildSetup()
{
    Invoke-Expression "& `".\WslToolbox.Setup\build.ps1`" ${PSScriptRoot}\WslToolbox.Gui\bin\Debug\net5.0-windows10.0.19041.0"
}

switch ($Component)
{
    "setup" {
        BuildSetup
    }
}