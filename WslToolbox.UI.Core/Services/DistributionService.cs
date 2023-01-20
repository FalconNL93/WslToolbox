using AutoMapper;
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
    private readonly UserOptions _userOptions;

    public DistributionService(ILogger<DistributionService> logger, IMapper mapper, IOptions<UserOptions> userOptions)
    {
        _logger = logger;
        _mapper = mapper;
        _userOptions = userOptions.Value;
    }

    public async Task RenameDistributions(UpdateModel<Distribution> distribution)
    {
        await RenameDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution.CurrentModel), distribution.NewModel.Name);
    }

    public async Task ExportDistribution(Distribution distribution, string path)
    {
        await ExportDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution), path);
    }

    public async Task ImportDistribution(NewDistributionModel model)
    {
        await ImportDistributionCommand.Execute(model.Name, model.InstallPath, model.File);
    }

    public async Task<IEnumerable<Distribution>> ListDistributions()
    {
        var distributions = await ListServiceCommand.ListDistributions(_userOptions.HideDocker);

        return _mapper.Map<IEnumerable<Distribution>>(distributions);
    }

    public async Task<IEnumerable<Distribution>> ListInstallableDistributions()
    {
        var currentDistributions = await ListServiceCommand.ListDistributions(_userOptions.HideDocker);
        var distributions = await DistributionClass.ListAvailableDistributions(currentDistributions.Where(x => x.IsInstalled).ToList());

        return _mapper.Map<IEnumerable<Distribution>>(distributions);
    }

    public void InstallDistribution(Distribution distribution)
    {
        OpenShellDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution));
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

    public async Task RestartDistribution(Distribution distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await TerminateDistributionCommand.Execute(distributionClass);
        await StartDistributionCommand.Execute(distributionClass);
    }

    public async Task DeleteDistribution(Distribution distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        await UnregisterDistributionCommand.Execute(distributionClass);
    }

    public void OpenShellDistribution(Distribution distribution)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        OpenShellDistributionCommand.Execute(distributionClass);
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

    public async Task<CommandClass> ExecuteCommand(Distribution distribution, string command)
    {
        var distributionClass = _mapper.Map<DistributionClass>(distribution);
        return await ExecuteDistributionCommand.Run(distributionClass, command);
    }
}