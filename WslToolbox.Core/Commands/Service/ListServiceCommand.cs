using System.Collections.Generic;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service;

public static class ListServiceCommand
{
    private const string Command = "wsl --list --verbose";

    public static async Task<List<DistributionClass>> ListDistributions(bool withoutDocker = false)
    {
        var distributionClass = new DistributionClass();
        var distributionListOutput = CommandClass.ExecuteCommand(Command);

        var distributionList = distributionClass.ListDistributions(distributionListOutput.Output);

        if (!withoutDocker)
        {
            return await Task.FromResult(distributionList).ConfigureAwait(true);
        }

        _ = distributionList.RemoveAll(distro => distro.Name == "docker-desktop");
        _ = distributionList.RemoveAll(distro => distro.Name == "docker-desktop-data");

        return await Task.FromResult(distributionList).ConfigureAwait(true);
    }
}