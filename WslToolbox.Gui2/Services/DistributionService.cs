using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Service;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.Services;

public class DistributionService
{
    private readonly ILogger<DistributionService> _logger;
    private readonly IMapper _mapper;

    public DistributionService(ILogger<DistributionService> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DistributionModel>> ListDistributions()
    {
        var distributions = await ListServiceCommand.ListDistributions();

        return _mapper.Map<IEnumerable<DistributionModel>>(distributions);
    }

    public async Task StartDistribution(DistributionModel distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await Core.Commands.Distribution.StartDistributionCommand.Execute(distributionClass);
    }
    
    public async Task StopDistribution(DistributionModel distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await Core.Commands.Distribution.TerminateDistributionCommand.Execute(distributionClass);
    }
}