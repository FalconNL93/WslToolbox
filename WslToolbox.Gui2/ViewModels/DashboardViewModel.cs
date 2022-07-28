using System.Collections.ObjectModel;
using System.Diagnostics;
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
    private readonly INavigationService _navigationService;
    private readonly ILogger<DashboardViewModel> _logger;
    private readonly DistributionService _distributionService;

    public RelayCommand AddDistribution { get; }
    public AsyncRelayCommand RefreshDistributions { get; }
    public ObservableCollection<DistributionModel> Distributions { get; } = new();

    private bool _canRefresh = true;
    private bool CanRefresh
    {
        get => _canRefresh;
        set
        {
            SetProperty(ref _canRefresh, value);
            RefreshDistributions.NotifyCanExecuteChanged();
        }
    }

    public DashboardViewModel(
        INavigationService navigationService,
        ILogger<DashboardViewModel> logger,
        DistributionService distributionService)
    {
        _navigationService = navigationService;
        _logger = logger;
        _distributionService = distributionService;

        RefreshDistributions = new AsyncRelayCommand(OnRefreshDistributions, () => CanRefresh);
        AddDistribution = new RelayCommand(OnAddDistribution, () => CanRefresh);
    }

    private void OnAddDistribution()
    {
    }

    private async Task OnRefreshDistributions()
    {
        Distributions.Clear();
        (await _distributionService.ListDistributions()).ToList().ForEach(distribution => { Distributions.Add(distribution); });
    }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }
}