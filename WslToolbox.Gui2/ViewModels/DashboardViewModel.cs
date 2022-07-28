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

    public RelayCommand AddDistribution => new(OnAddDistribution);

    public AsyncRelayCommand RefreshDistributions => new(OnRefreshDistributions, CanEdit);
    public ObservableCollection<DistributionModel> Distributions { get; set; } = new();

    public DashboardViewModel(
        INavigationService navigationService,
        ILogger<DashboardViewModel> logger,
        DistributionService distributionService)
    {
        _navigationService = navigationService;
        _logger = logger;
        _distributionService = distributionService;
    }

    private async void OnStopDistribution(string? parameter)
    {
    }

    private void OnAddDistribution()
    {
    }

    private bool CanEdit()
    {
        return Distributions.Count <= 2;
    }

    private async Task OnRefreshDistributions()
    {
        Distributions.Clear();
        (await _distributionService.ListDistributions()).ToList().ForEach(distribution =>
        {
            Distributions.Add(distribution);
        });
    }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }
}