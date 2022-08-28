using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Contracts.Services;

public interface IDistributionDataService
{
    Task<IEnumerable<Distribution>> GetDistributions();
}