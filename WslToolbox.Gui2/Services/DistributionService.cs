using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Core.Commands.Service;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.Services;

public class DistributionService
{
    public const string StateRunning = "Running";
    public const string StateStopped = "Stopped";
    public const string StateAvailable = "Stopped";

    private readonly ILogger<DistributionService> _logger;
    private readonly IMapper _mapper;
    private readonly AppConfig _options;

    public DistributionService(ILogger<DistributionService> logger, IMapper mapper, IOptions<AppConfig> options)
    {
        _logger = logger;
        _mapper = mapper;
        _options = options.Value;
    }

    public async Task<IEnumerable<DistributionModel>> ListDistributions()
    {
        var distributions = await ListServiceCommand.ListDistributions(_options.HideDockerDist);

        return _mapper.Map<IEnumerable<DistributionModel>>(distributions);
    }

    public async Task StartDistribution(DistributionModel distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await StartDistributionCommand.Execute(distributionClass);
    }

    public async Task StopDistribution(DistributionModel distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await TerminateDistributionCommand.Execute(distributionClass);
    }
}