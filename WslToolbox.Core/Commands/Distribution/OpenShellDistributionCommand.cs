using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class OpenShellDistributionCommand
    {
        private const int MaximumInstallTries = 20;
        public static event EventHandler OpenShellInstallDistributionFinished;

        public static async void Execute(DistributionClass distribution)
        {
            CommandClass.StartShell(distribution);

            if (distribution.IsInstalled) return;

            int installTries = 0;
            while (!ToolboxClass.DistributionByName(distribution.Name).IsInstalled)
            {
                if (installTries >= MaximumInstallTries) return;
                
                await Task.Delay(5000);
                installTries++;
            }

            OpenShellInstallDistributionFinished?.Invoke(distribution, EventArgs.Empty);
        }
    }
}