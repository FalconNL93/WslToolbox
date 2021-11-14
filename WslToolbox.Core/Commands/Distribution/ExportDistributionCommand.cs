using System;
using System.Threading.Tasks;
using WslToolbox.Core.EventArguments;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class ExportDistributionCommand
    {
        private const string Command = "wsl --export {0} {1}";

        public static event EventHandler DistributionExportStarted;
        public static event EventHandler DistributionExportFinished;

        public static async Task<CommandClass> Execute(DistributionClass distribution, string file)
        {
            var args = new DistributionEventArguments(nameof(ExportDistributionCommand), distribution);

            DistributionExportStarted?.Invoke(typeof(ExportDistributionCommand), args);
            var exportTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command, distribution.Name, file
            ))).ConfigureAwait(true);
            DistributionExportFinished?.Invoke(typeof(ExportDistributionCommand), args);

            return exportTask;
        }
    }
}