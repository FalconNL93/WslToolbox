using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using WslToolbox.Gui2.Services;

namespace WslToolbox.Gui2.ViewModels;

public class ServiceViewModel : ObservableObject
{
    private readonly ILogger<ServiceViewModel> _logger;
    public AsyncRelayCommand FetchServiceStatus { get; }
    public AsyncRelayCommand StartService { get; }
    public AsyncRelayCommand StopService { get; }
    public AsyncRelayCommand RestartService { get; }

    private bool _isRunning;

    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    public ServiceViewModel(ILogger<ServiceViewModel> logger)
    {
        _logger = logger;

        FetchServiceStatus = new AsyncRelayCommand(OnFetchServiceStatus);
        StartService = new AsyncRelayCommand(OnStartService, () => !IsRunning);
        StopService = new AsyncRelayCommand(OnStopService, () => IsRunning);
        RestartService = new AsyncRelayCommand(OnRestartService, () => IsRunning);

        FetchServiceStatus.ExecuteAsync(null);
    }

    private async Task OnFetchServiceStatus() => IsRunning = await DistributionService.ServiceStatus();

    private async Task OnStartService()
    {
        await DistributionService.ServiceStart();
        await FetchServiceStatus.ExecuteAsync(null);
    }

    private async Task OnStopService()
    {
        await DistributionService.ServiceStop();
        await FetchServiceStatus.ExecuteAsync(null);
    }

    private async Task OnRestartService()
    {
        await DistributionService.ServiceRestart();
        await FetchServiceStatus.ExecuteAsync(null);
    }
}