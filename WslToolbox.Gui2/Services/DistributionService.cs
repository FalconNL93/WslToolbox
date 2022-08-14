using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public void RenameDistributions(UpdateModel<DistributionModel> distribution)
    {
        RenameDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution.CurrentModel),
            distribution.NewModel.Name);
    }

    public async Task ExportDistribution(DistributionModel distribution)
    {
        await ExportDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution),
            "C:\\Users\\Peter\\Downloads\\export\\blabla.tar.gz");
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

    public async Task DeleteDistribution(DistributionModel distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await UnregisterDistributionCommand.Execute(distributionClass);
    }

    public async Task<string> OptimizeDistribution(DistributionModel distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);

        _logger.LogInformation("Searching for VHDX files in {BasePath}", distribution.BasePath);
        var virtualFileSystem = Directory.GetFiles(distribution.BasePath, "*.vhdx", SearchOption.TopDirectoryOnly);
        _logger.LogInformation("Found virtual filesystems: {FileSystems}", virtualFileSystem.ToList());
        _logger.LogInformation("Stopping WSL Services");

        _logger.LogInformation("Optimizing {FileSystem}.vhdx", virtualFileSystem.FirstOrDefault());
        try
        {
            await StopServiceCommand.Execute();
            await OptimizeDistributionCommand.Execute(
                distributionClass,
                virtualFileSystem.FirstOrDefault(),
                $"{App.AppDirectory}/logs/diskpart-{distribution.Name}.log");
        }
        catch (Exception e)
        {
            throw new Exception("Could not optimize distribution", e);
        }


        return $"{App.AppDirectory}/logs/diskpart-{distribution.Name}.log";
    }
}