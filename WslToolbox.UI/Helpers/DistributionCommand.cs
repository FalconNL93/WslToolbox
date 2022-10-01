using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Helpers;

public static class DistributionCommand
{
    public static bool CanStartDistribution(Distribution? distribution)
    {
        if (distribution == null)
        {
            return false;
        }

        return distribution.State == Distribution.StateStopped;
    }

    public static bool CanStopDistribution(Distribution? distribution)
    {
        if (distribution == null)
        {
            return false;
        }

        return distribution.State == Distribution.StateRunning;
    }

    public static bool CanRestartDistribution(Distribution? distribution)
    {
        if (distribution == null)
        {
            return false;
        }

        return distribution.State != Distribution.StateBusy;
    }
}