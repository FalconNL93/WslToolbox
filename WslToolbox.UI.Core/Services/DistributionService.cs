﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Core.Commands.Service;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Services;

public class DistributionService
{
    public const string StateRunning = "Running";
    public const string StateStopped = "Stopped";
    public const string StateAvailable = "Stopped";

    private readonly ILogger<DistributionService> _logger;
    private readonly IMapper _mapper;

    public DistributionService(ILogger<DistributionService> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public void RenameDistributions(UpdateModel<Distribution> distribution)
    {
        RenameDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution.CurrentModel),
            distribution.NewModel.Name);
    }

    public async Task ExportDistribution(Distribution distribution)
    {
        await ExportDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution),
            "C:\\Users\\Peter\\Downloads\\export\\blabla.tar.gz");
    }

    public async Task<IEnumerable<Distribution>> ListDistributions()
    {
        var distributions = await ListServiceCommand.ListDistributions();

        return _mapper.Map<IEnumerable<Distribution>>(distributions);
    }

    public async Task StartDistribution(Distribution distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await StartDistributionCommand.Execute(distributionClass);
    }

    public async Task StopDistribution(Distribution distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await TerminateDistributionCommand.Execute(distributionClass);
    }

    public async Task DeleteDistribution(Distribution distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await UnregisterDistributionCommand.Execute(distributionClass);
    }

    public async Task<string> OptimizeDistribution(Distribution distribution)
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
                $"logs/diskpart-{distribution.Name}.log");
        }
        catch (Exception e)
        {
            throw new Exception("Could not optimize distribution", e);
        }


        return $"logs/diskpart-{distribution.Name}.log";
    }

    public static async Task<bool> ServiceStatus()
    {
        var serviceStatus = await StatusServiceCommand.ServiceIsRunning();

        return serviceStatus;
    }

    public static async Task ServiceStart()
    {
        await StartServiceCommand.Execute();
    }

    public static async Task ServiceStop()
    {
        await StopServiceCommand.Execute();
    }

    public static async Task ServiceRestart()
    {
        await StopServiceCommand.Execute();
        await StartServiceCommand.Execute();
    }
}