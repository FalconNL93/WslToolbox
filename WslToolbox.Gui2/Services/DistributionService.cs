using Microsoft.Extensions.Logging;

namespace WslToolbox.Gui2.Services;

public class DistributionService
{
    private readonly ILogger<DistributionService> _logger;

    public DistributionService(ILogger<DistributionService> logger)
    {
        _logger = logger;
    }

    public async void ListDistributions()
    {
        var distr = await Core.Commands.Service.ListServiceCommand.ListDistributions();
        
        
    }
}