using System.Collections.Generic;
using System.Threading.Tasks;

namespace WslToolbox
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
            UnregisterDistribution = "wsl --unregister"
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

        public static DistributionClass DefaultDistribution() => ListDistributions().Find(distro => distro.IsDefault);

        public static DistributionClass ByName(string name) => ListDistributions().Find(distro => distro.Name == name);

        public static List<DistributionClass> ListDistributions()
        {
            CommandClass distributionListOutput = CommandClass.ExecuteCommand(WslCommands.List);

            return DistributionClass.FromOutput(distributionListOutput.Output);
        }

        public static List<DistributionClass> ListAvailableDistributions()
        {
            CommandClass distributionListOutput = CommandClass.ExecuteCommand(WslCommands.ListAvailable);

            return DistributionClass.FromAvailableOutput(distributionListOutput.Output);
        }
    }
}