using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class OpenShellDistributionCommand
    {
        private const int MaximumInstallTries = 20;
        private const int RefreshRateInstall = 2000;
        private const int RefreshRatePostInstall = 2000;
        public static event EventHandler OpenShellInstallDistributionFinished;

        public static async void Execute(DistributionClass distribution)
        {
            var openShell = await CommandClass.StartShellAsync(distribution);

            if (openShell.ExitCode != 0)
            {
                return;
            }

            if (distribution.IsInstalled)
            {
                return;
            }

            ToolboxClass.OnRefreshRequired(2000);
            if (ToolboxClass.DistributionByName(distribution.Name) != null)
            {
                return;
            }

            var installTries = 0;
            while (ToolboxClass.DistributionByName(distribution.Name) == null ||
                   ToolboxClass.DistributionByName(distribution.Name).IsInstalled == false)
            {
                Debug.WriteLine($"Checking install status attempt {installTries}/{MaximumInstallTries}");
                if (installTries >= MaximumInstallTries)
                {
                    return;
                }

                await Task.Delay(RefreshRateInstall);
                installTries++;
            }

            await Task.Delay(RefreshRatePostInstall);
            Debug.WriteLine($"Install finished after {installTries}/{MaximumInstallTries} attempts");
            ToolboxClass.OnRefreshRequired();
            OpenShellInstallDistributionFinished?.Invoke(distribution, EventArgs.Empty);
        }
    }
}