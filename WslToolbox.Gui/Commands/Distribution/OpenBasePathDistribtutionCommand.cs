using System.IO;
using WslToolbox.Core;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class OpenBasePathDistribution : GenericDistributionCommand
    {
        public OpenBasePathDistribution(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => Directory.Exists(distributionClass.BasePathLocal);
            IsExecutable = IsExecutableDefault;
        }

        public override void Execute(object parameter)
        {
            var distribution = (DistributionClass) parameter;

            if (!Directory.Exists(distribution.BasePathLocal)) return;

            ExplorerHelper.OpenLocal(distribution.BasePathLocal);
        }
    }
}