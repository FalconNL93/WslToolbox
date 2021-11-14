using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class ListServiceCommand
    {
        private const string Command = "wsl --list --verbose";
        private const string CommandOnline = "wsl --list --online";

        public static async Task<List<DistributionClass>> ListDistributions(bool withoutDocker = false)
        {
            var distributionListOutput = CommandClass.ExecuteCommand(Command);
            var distributionAvailableListOutput = CommandClass.ExecuteCommand(CommandOnline);

            var distributionList = DistributionClass.FromOutput(distributionListOutput.Output);
            var distributionListAvailable =
                DistributionClass.FromAvailableOutput(distributionAvailableListOutput.Output);

            distributionList.AddRange(distributionListAvailable
                .Where(dist1 => distributionList.All(dist2 => dist2.Name != dist1.Name)));

            if (!withoutDocker) return await Task.FromResult(distributionList).ConfigureAwait(true);
            _ = distributionList.RemoveAll(distro => distro.Name == "docker-desktop");
            _ = distributionList.RemoveAll(distro => distro.Name == "docker-desktop-data");

            return await Task.FromResult(distributionList).ConfigureAwait(true);
        }
    }
}