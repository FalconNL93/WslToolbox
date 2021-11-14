using System.Diagnostics;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class StatusServiceCommand
    {
        private const string Command = "wsl --status";

        public static async Task<CommandClass> Execute()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command
            ))).ConfigureAwait(true);
        }

        public static async Task<bool> ServiceIsRunning()
        {
            return await Task.Run(() => Process.GetProcessesByName("wslhost").Length > 0).ConfigureAwait(true);
        }
    }
}