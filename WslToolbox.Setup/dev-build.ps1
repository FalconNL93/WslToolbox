# Environment Variables
$Env:ProductName = "WSL Toolbox"
$Env:ProductVersion = "0.5.999.9"
$Env:ProductDescription = "WSL Toolbox allows you to manage your WSL Distributions through an easy to use interface."
$Env:ProductEnvironment = "Debug"
$Env:ProductUuid = "FalconNL93.WSLToolbox"
$Env:OutputFile = "wsltoolbox-$(Env:ProductVersion)-setup"
$Env:TargetFramework = "net5.0-windows10.0.19041.0"

# Call Build
.\build.ps1 "D:\CSharp\WslToolbox\WslToolbox.Gui\bin\Debug\net5.0-windows10.0.19041.0"