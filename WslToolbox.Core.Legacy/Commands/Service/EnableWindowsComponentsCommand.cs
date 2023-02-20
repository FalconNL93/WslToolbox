using System.Collections.Generic;
using System.Threading.Tasks;

namespace WslToolbox.Core.Legacy.Commands.Service;

public static class EnableWindowsComponentsCommand
{
    private const string ShellBackend = "powershell.exe";

    private static readonly IEnumerable<string> EnableCommands = new[]
    {
        "dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart",
        "dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart"
    };

    public static async Task Execute()
    {
        var enableCommand = string.Join(";", EnableCommands);

        await Task.Run(() => CommandClass.ExecuteCommand(
            enableCommand, elevated: true, executable: ShellBackend)).ConfigureAwait(true);
    }
}