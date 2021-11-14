using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class UpdateServiceCommand
    {
        private const string Command = "wsl --update";

        public static async Task<CommandClass> Execute()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command
            ))).ConfigureAwait(true);
        }
    }
}