using System.Diagnostics;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class EnableWindowsComponentsCommand
    {
        private const string EnableCommand =
            "dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart";

        private const string ShellBackend = "powershell.exe";

        public static async Task Execute()
        {
            var task = await Task.Run(() => CommandClass.ExecuteCommand(
                EnableCommand, elevated: true, executable: ShellBackend)).ConfigureAwait(true);

            Debug.WriteLine(task.ExitCode);
        }
    }
}