using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.Services;

namespace WslToolbox.Gui2.ViewModels;

public class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly DistributionService _distributionService;
    private readonly ILogger<DashboardViewModel> _logger;
    private readonly INavigationService _navigationService;

    private bool _canRefresh = true;

    public DashboardViewModel(
        INavigationService navigationService,
        ILogger<DashboardViewModel> logger,
        DistributionService distributionService)
    {
        _navigationService = navigationService;
        _logger = logger;
        _distributionService = distributionService;

        RefreshDistributions = new AsyncRelayCommand(OnRefreshDistributions, () => CanRefresh);
        AddDistribution = new RelayCommand(OnAddDistribution, () => false);

        StartDistribution = new AsyncRelayCommand<DistributionModel>(OnStartDistribution, model => model?.State != "Running");
        StopDistribution = new AsyncRelayCommand<DistributionModel>(OnStopDistribution, model => model?.State == "Running");
    }

    public RelayCommand AddDistribution { get; }
    public AsyncRelayCommand RefreshDistributions { get; }
    public AsyncRelayCommand<DistributionModel> StartDistribution { get; }
    public AsyncRelayCommand<DistributionModel> StopDistribution { get; }
    public ObservableCollection<DistributionModel> Distributions { get; } = new();

    private bool CanRefresh
    {
        get => _canRefresh;
        set
        {
            SetProperty(ref _canRefresh, value);
            RefreshDistributions.NotifyCanExecuteChanged();
        }
    }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnAddDistribution()
    {
    }

    private async Task OnStartDistribution(DistributionModel? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        await _distributionService.StartDistribution(distribution);
    }

    private async Task OnStopDistribution(DistributionModel? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        await _distributionService.StopDistribution(distribution);
    }

    private async Task OnRefreshDistributions()
    {
        Distributions.Clear();
        (await _distributionService.ListDistributions()).ToList()
            .ForEach(distribution => { Distributions.Add(distribution); });
    }
}