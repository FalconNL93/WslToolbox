using WslToolbox.UI.Core.Contracts.Services;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Services;

public class DistributionDataService : IDistributionDataService
{
    private List<Distribution> _distributions;

    public async Task<IEnumerable<Distribution>> GetDistributions()
    {
        _distributions ??= new List<Distribution>
        {
            new()
            {
                IsDefault = false,
                IsInstalled = true,
                Name = "TestDist",
                State = "Installed",
                Version = 2,
                Guid = "blablal",
                BasePath = "blablab",
                BasePathLocal = "bawdawd",
                DefaultUid = 0,
                Size = 300
            }
        };

        await Task.CompletedTask;
        return _distributions;
    }
}