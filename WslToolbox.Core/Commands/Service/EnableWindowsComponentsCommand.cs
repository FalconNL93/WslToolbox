using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class EnableWindowsComponentsCommand
    {
        private static readonly IEnumerable<string> EnableCommands = new[]
        {
            "dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart",
            "dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart"
        };

        private const string ShellBackend = "powershell.exe";

        public static async Task Execute()
        {
            var enableCommand = string.Join(";", EnableCommands);

            var task = await Task.Run(() => CommandClass.ExecuteCommand(
                enableCommand, elevated: true, executable: ShellBackend)).ConfigureAwait(true);

            Debug.WriteLine(task.ExitCode);
        }
    }
}