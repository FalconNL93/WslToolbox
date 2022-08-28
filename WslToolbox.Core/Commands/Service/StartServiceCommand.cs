using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service;

public static class StartServiceCommand
{
    private const string Command = "wsl --exec exit";

    public static event EventHandler ServiceStartFinished;

    public static async Task Execute()
    {
        await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
            Command
        ))).ConfigureAwait(true);

        ToolboxClass.OnRefreshRequired();
        ServiceStartFinished?.Invoke(null, EventArgs.Empty);
    }
}