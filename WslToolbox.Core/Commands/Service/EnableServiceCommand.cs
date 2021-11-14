using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class EnableServiceCommand
    {
        private const string Command =
            "Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux";

        public static async Task<CommandClass> Execute()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command
            ), elevated: true, executable: "powershell.exe")).ConfigureAwait(true);
        }
    }
}