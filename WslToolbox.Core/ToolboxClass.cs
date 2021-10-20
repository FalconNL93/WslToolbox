using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WslToolbox.Core.Helpers;

namespace WslToolbox.Core
{
    public static class ToolboxClass
    {
        public static DistributionClass ByName(string name)
        {
            return ListDistributions().Result.Find(distro => distro.Name == name);
        }

        public static async Task<CommandClass> ConvertDistribution(DistributionClass distribution)
        {
            return await Task
                .Run(() => CommandClass.ExecuteCommand($"{WslCommands.ConvertDistribution} {distribution.Name} 2"))
                .ConfigureAwait(true);
        }

        public static DistributionClass DefaultDistribution()
        {
            return ListDistributions().Result.Find(distro => distro.IsDefault);
        }

        public static async Task<CommandClass> ExportDistribution(DistributionClass distribution, string file)
        {
            return await Task
                .Run(() => CommandClass.ExecuteCommand($"{WslCommands.ExportDistribution} {distribution.Name} {file}"))
                .ConfigureAwait(true);
        }

        public static async Task<CommandClass> ImportDistribution(DistributionClass distribution, string name,
            string path, string file)
        {
            return await Task
                .Run(() => CommandClass.ExecuteCommand($"{WslCommands.ImportDistribution} {name} {path} {file}"))
                .ConfigureAwait(true);
        }

        public static async Task<List<DistributionClass>> ListDistributions(bool withoutDocker = false)
        {
            var distributionListOutput = CommandClass.ExecuteCommand(WslCommands.List);
            var distributionAvailableListOutput = CommandClass.ExecuteCommand(WslCommands.ListAvailable);

            var distributionList = DistributionClass.FromOutput(distributionListOutput.Output);
            var distributionListAvailable =
                DistributionClass.FromAvailableOutput(distributionAvailableListOutput.Output);

            distributionList.AddRange(distributionListAvailable
                .Where(dist1 => !distributionList.Any(dist2 => dist2.Name == dist1.Name)));

            if (withoutDocker)
            {
                _ = distributionList.RemoveAll(distro => distro.Name == "docker-desktop");
                _ = distributionList.RemoveAll(distro => distro.Name == "docker-desktop-data");
            }

            return await Task.FromResult(distributionList).ConfigureAwait(true);
        }

        public static async Task<CommandClass> SetDefaultDistribution(DistributionClass distribution)
        {
            return await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.SetDefault} {distribution.Name}"))
                .ConfigureAwait(true);
        }

        public static void ShellDistribution(DistributionClass distribution)
        {
            CommandClass.StartShell(distribution);
        }

        public static async Task<CommandClass> StartDistribution(DistributionClass distribution)
        {
            return await Task
                .Run(() => CommandClass.ExecuteCommand(
                    $"{WslCommands.StartDistribution} {distribution.Name} --exec exit")).ConfigureAwait(true);
        }

        public static async Task<CommandClass> StartWsl()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Start)).ConfigureAwait(true);
        }

        public static async Task<CommandClass> StatusWsl()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Status)).ConfigureAwait(true);
        }

        public static async Task<CommandClass> StopWsl()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Stop)).ConfigureAwait(true);
        }

        public static async Task<CommandClass> TerminateDistribution(DistributionClass distribution)
        {
            return await Task
                .Run(() => CommandClass.ExecuteCommand($"{WslCommands.TerminateDistribution} {distribution.Name}"))
                .ConfigureAwait(true);
        }

        public static async void RenameDistribution(DistributionClass distribution, string newName)
        {
            await TerminateDistribution(distribution);

            await Task
                .Run(() => { RegistryHelper.ChangeKey(distribution, "DistributionName", newName); })
                .ConfigureAwait(true);

            await StartDistribution(distribution);
        }

        public static async Task<CommandClass> UnregisterDistribution(DistributionClass distribution)
        {
            return await Task
                .Run(() => CommandClass.ExecuteCommand($"{WslCommands.UnregisterDistribution} {distribution.Name}"))
                .ConfigureAwait(true);
        }

        public static async Task<CommandClass> UpdateWsl()
        {
            return await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Update)).ConfigureAwait(true);
        }

        public static async Task<bool> ServiceIsRunning()
        {
            return await Task.Run(() => Process.GetProcessesByName("wslhost").Length > 0).ConfigureAwait(true);
        }
    }

    internal static class WslCommands
    {
        public const string
            Start = "wsl --exec exit",
            Stop = "wsl --shutdown",
            Update = "wsl --update",
            Status = "wsl --status",
            List = "wsl --list --verbose",
            ListAvailable = "wsl --list --online",
            SetDefault = "wsl --set-default",
            TerminateDistribution = "wsl --terminate",
            StartDistribution = "wsl --distribution",
            UnregisterDistribution = "wsl --unregister",
            ConvertDistribution = "wsl --set-version",
            ExportDistribution = "wsl --export",
            ImportDistribution = "wsl --import";
    }
}