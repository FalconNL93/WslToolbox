function DotNetClean() {
    try {
        $DotnetArgs = @()
        $DotnetArgs = $DotnetArgs + "clean"
        $DotnetArgs = $DotnetArgs + ".\WslToolbox.sln"
        & dotnet $DotnetArgs
        & nuget locals all -clear
    }
    catch {
        throw $_
    }

}

function DotNetBuild() {
    $DotnetArgs = @()
    $DotnetArgs = $DotnetArgs + "build"
    $DotnetArgs = $DotnetArgs + ".\WslToolbox.sln"
    $DotnetArgs = $DotnetArgs + "--configuration" + "Release"
    & dotnet $DotnetArgs
}

function DotNetTools() {
    dotnet tool install --global dotnet-ef
    dotnet tool install --global wix
    dotnet tool update -g --no-cache --all
    dotnet workload update
}

Get-ChildItem .\ -include bin, obj -Recurse | ForEach-Object ($_) { 
    Write-Output "Removing $_...";
    remove-item $_.fullname -Force -Recurse
}

try {
    DotNetClean
    DotNetTools
}
catch {
    throw $_
}
