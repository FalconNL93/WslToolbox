using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class StopServiceCommand
    {
        private const string Command = "wsl --shutdown";

        public static async Task<CommandClass> Execute()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command
            ))).ConfigureAwait(true);
        }
    }
}