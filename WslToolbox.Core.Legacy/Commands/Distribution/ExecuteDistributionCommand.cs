using System.Threading.Tasks;

namespace WslToolbox.Core.Legacy.Commands.Distribution;

public static class ExecuteDistributionCommand
{
    private const string Command = "wsl -d {0} {1}";

    public static async Task<CommandClass> Run(DistributionClass distribution, string command)
    {
        var commandFormatted = string.Format(Command, distribution.Name, command);
        var unregisterTask = await Task.Run(() => CommandClass.ExecuteCommand(commandFormatted)).ConfigureAwait(true);
        ToolboxClass.OnRefreshRequired();

        return unregisterTask;
    }
}