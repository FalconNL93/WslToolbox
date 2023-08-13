using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.Core.Legacy;
using WslToolbox.Core.Legacy.Commands.Distribution;
using WslToolbox.Core.Legacy.Commands.Service;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Services;

public class DistributionService
{
    public const string StateRunning = "Running";
    public const string StateStopped = "Stopped";
    public const string StateAvailable = "Stopped";
    public const string StateBusy = "Busy";

    private readonly ILogger<DistributionService> _logger;
    private readonly IMapper _mapper;
    private readonly UserOptions _userOptions;
    private readonly IMessenger _messenger;

    public DistributionService(ILogger<DistributionService> logger, IMapper mapper, IOptions<UserOptions> userOptions, IMessenger messenger)
    {
        _logger = logger;
        _mapper = mapper;
        _messenger = messenger;
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
    
    public async Task MoveDistribution(Distribution distribution, string path)
    {
        try
        {
            distribution.State = StateBusy;
            await IoService.CopyDirectory(distribution.BasePath, path);
            ChangeBasePathDistributionCommand.Execute(_mapper.Map<DistributionClass>(distribution), path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not move distribution");
            throw;
        }
        finally
        {
            distribution.State = StateAvailable;
        }
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

        var shellBehaviour = _userOptions.ShellBehaviour;
        OpenShellDistributionCommand.Execute(distributionClass, shellBehaviour);
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
    
    public Dictionary<int, string> ShellBehaviours { get; set; } = new()
    {
        { 0, "Default" },
        { 1, "Home" },
        { 2, "Current" }
    };
}