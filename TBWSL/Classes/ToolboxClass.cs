using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WslToolbox.Classes
{
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
            ImportDistribution = "wsl --import"
        ;
    }

    internal static class ToolboxClass
    {
        public static async Task<CommandClass> StartWsl() => await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Start));

        public static async Task<CommandClass> StopWsl() => await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Stop));

        public static async Task<CommandClass> UpdateWsl() => await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Update));

        public static async Task<CommandClass> StatusWsl() => await Task.Run(() => CommandClass.ExecuteCommand(WslCommands.Status));

        public static async Task<CommandClass> SetDefaultDistribution(DistributionClass distribution) => await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.SetDefault} {distribution.Name}"));

        public static async Task<CommandClass> TerminateDistribution(DistributionClass distribution) => await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.TerminateDistribution} {distribution.Name}"));

        public static async Task<CommandClass> UnregisterDistribution(DistributionClass distribution) => await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.UnregisterDistribution} {distribution.Name}"));

        public static async Task<CommandClass> StartDistribution(DistributionClass distribution) => await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.StartDistribution} {distribution.Name} --exec exit"));

        public static void ShellDistribution(DistributionClass distribution) => CommandClass.StartShell(distribution);

        public static async Task<CommandClass> ConvertDistribution(DistributionClass distribution) => await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.ConvertDistribution} {distribution.Name} 2"));

        public static DistributionClass DefaultDistribution() => ListDistributions().Find(distro => distro.IsDefault);

        public static DistributionClass ByName(string name) => ListDistributions().Find(distro => distro.Name == name);

        public static async Task<CommandClass> ExportDistribution(DistributionClass distribution, string file)
        {
            return await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.ExportDistribution} {distribution.Name} {file}"));
        }

        public static async Task<CommandClass> ImportDistribution(DistributionClass distribution, string name, string path, string file)
        {
            return await Task.Run(() => CommandClass.ExecuteCommand($"{WslCommands.ImportDistribution} {name} {path} {file}"));
        }

        public static List<DistributionClass> ListDistributions()
        {
            CommandClass distributionListOutput = CommandClass.ExecuteCommand(WslCommands.List);
            CommandClass distributionAvailableListOutput = CommandClass.ExecuteCommand(WslCommands.ListAvailable);

            List<DistributionClass> distributionList = DistributionClass.FromOutput(distributionListOutput.Output);
            List<DistributionClass> distributionListAvailable = DistributionClass.FromAvailableOutput(distributionAvailableListOutput.Output);

            distributionList.AddRange(distributionListAvailable.Where(dist1 => !distributionList.Any(dist2 => dist2.Name == dist1.Name)));

            return distributionList;
        }
    }
}