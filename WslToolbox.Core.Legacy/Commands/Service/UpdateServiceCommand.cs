using System.Threading.Tasks;

namespace WslToolbox.Core.Legacy.Commands.Service;

public static class UpdateServiceCommand
{
    private const string Command = "wsl --update";

    public static async Task Execute()
    {
        await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
            Command
        ))).ConfigureAwait(true);
    }
}