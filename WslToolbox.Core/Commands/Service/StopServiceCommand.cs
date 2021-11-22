using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class StopServiceCommand
    {
        private const string Command = "wsl --shutdown";

        public static event EventHandler ServiceStopFinished;

        public static async Task Execute()
        {
            await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command
            ))).ConfigureAwait(true);

            ToolboxClass.OnRefreshRequired();
            ServiceStopFinished?.Invoke(null, EventArgs.Empty);
        }
    }
}