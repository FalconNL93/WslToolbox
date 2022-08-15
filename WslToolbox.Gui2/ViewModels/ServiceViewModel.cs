using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using WslToolbox.Gui2.Services;

namespace WslToolbox.Gui2.ViewModels;

public class ServiceViewModel : ObservableObject
{
    private readonly ILogger<ServiceViewModel> _logger;

    private bool _isRunning;

    public ServiceViewModel(ILogger<ServiceViewModel> logger)
    {
        _logger = logger;

        FetchServiceStatus = new AsyncRelayCommand(OnFetchServiceStatus);
        StartService = new AsyncRelayCommand(OnStartService, () => !IsRunning);
        StopService = new AsyncRelayCommand(OnStopService, () => IsRunning);
        RestartService = new AsyncRelayCommand(OnRestartService, () => IsRunning);

        FetchServiceStatus.ExecuteAsync(null);
    }

    public AsyncRelayCommand FetchServiceStatus { get; }
    public AsyncRelayCommand StartService { get; set; }
    public AsyncRelayCommand StopService { get; set; }
    public AsyncRelayCommand RestartService { get; }

    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            SetProperty(ref _isRunning, value);
            StartService.NotifyCanExecuteChanged();
            StopService.NotifyCanExecuteChanged();
            RestartService.NotifyCanExecuteChanged();
        }
    }

    private async Task OnFetchServiceStatus()
    {
        IsRunning = await DistributionService.ServiceStatus();
    }

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