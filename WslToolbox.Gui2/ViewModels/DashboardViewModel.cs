using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Wpf.Ui.Common.Interfaces;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.Services;

namespace WslToolbox.Gui2.ViewModels;

public class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly ILogger<DashboardViewModel> _logger;
    private readonly DistributionService _service;

    public DashboardViewModel(
        ILogger<DashboardViewModel> logger,
        DistributionService service)
    {
        _logger = logger;
        _service = service;

        RefreshDistributions = new AsyncRelayCommand(OnRefreshDistributions);
        StartDistribution = new AsyncRelayCommand<DistributionModel>(OnStartDistribution, model => model?.State != DistributionService.StateRunning);
        StopDistribution = new AsyncRelayCommand<DistributionModel>(OnStopDistribution, model => model?.State == DistributionService.StateRunning);
    }

    public AsyncRelayCommand RefreshDistributions { get; }
    public AsyncRelayCommand<DistributionModel> StartDistribution { get; }
    public AsyncRelayCommand<DistributionModel> StopDistribution { get; }
    public ObservableCollection<DistributionModel> Distributions { get; } = new();

    public async void OnNavigatedTo()
    {
        await RefreshDistributions.ExecuteAsync(null);
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task OnStartDistribution(DistributionModel? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        await _service.StartDistribution(distribution);
    }

    private async Task OnStopDistribution(DistributionModel? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        await _service.StopDistribution(distribution);
    }

    private async Task OnRefreshDistributions()
    {
        Distributions.Clear();
        (await _service.ListDistributions()).ToList()
            .ForEach(distribution => { Distributions.Add(distribution); });
    }
}