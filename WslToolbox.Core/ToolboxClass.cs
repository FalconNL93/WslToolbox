using WslToolbox.Core.Commands.Service;

namespace WslToolbox.Core
{
    public static class ToolboxClass
    {
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
    }
}