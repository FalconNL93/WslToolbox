using System;
using System.Threading.Tasks;
using WslToolbox.Core.Legacy.Commands.Service;

namespace WslToolbox.Core.Legacy;

public static class ToolboxClass
{
    public static event EventHandler RefreshRequired;

    public static DistributionClass DistributionByName(string name)
    {
        return ListServiceCommand.ListDistributions().Result
            .Find(distro => distro.Name == name);
    }

    public static DistributionClass DefaultDistribution()
    {
        return ListServiceCommand.ListDistributions().Result
            .Find(distro => distro.IsDefault);
    }

    public static async void OnRefreshRequired(int delay = 0)
    {
        await Task.Delay(delay);
        RefreshRequired?.Invoke(null, EventArgs.Empty);
    }
}