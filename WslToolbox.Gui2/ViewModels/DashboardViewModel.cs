using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.Services;

namespace WslToolbox.Gui2.ViewModels;

public class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly ILogger<DashboardViewModel> _logger;
    private readonly DistributionService _service;
    private readonly ISnackbarService _snackbarService;

    public DashboardViewModel(
        ILogger<DashboardViewModel> logger,
        DistributionService service,
        ISnackbarService snackbarService
    )
    {
        _logger = logger;
        _service = service;
        _snackbarService = snackbarService;

        RefreshDistributions = new AsyncRelayCommand(OnRefreshDistributions);
        StartDistribution = new AsyncRelayCommand<DistributionModel>(OnStartDistribution, model => model?.State != DistributionService.StateRunning);
        StopDistribution = new AsyncRelayCommand<DistributionModel>(OnStopDistribution, model => model?.State == DistributionService.StateRunning);
        DeleteDistribution = new AsyncRelayCommand<DistributionModel>(OnDeleteDistribution);
        ExportDistribution = new AsyncRelayCommand<DistributionModel>(OnExportDistribution);
        EditDistribution = new RelayCommand<UpdateModel<DistributionModel>>(OnEditDistribution);
    }

    public AsyncRelayCommand RefreshDistributions { get; }
    public AsyncRelayCommand<DistributionModel> StartDistribution { get; }
    public AsyncRelayCommand<DistributionModel> StopDistribution { get; }
    public AsyncRelayCommand<DistributionModel> DeleteDistribution { get; }
    public RelayCommand<UpdateModel<DistributionModel>> EditDistribution { get; }
    public AsyncRelayCommand<DistributionModel> ExportDistribution { get; }
    public ObservableCollection<DistributionModel> Distributions { get; } = new();

    public async void OnNavigatedTo()
    {
        await RefreshDistributions.ExecuteAsync(null);
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnEditDistribution(UpdateModel<DistributionModel>? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        _service.RenameDistributions(distribution);
    }

    private async Task OnDeleteDistribution(DistributionModel? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        await _service.DeleteDistribution(distribution);
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

    private async Task OnExportDistribution(DistributionModel? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        _snackbarService.Show("Exporting", "Distribution is exporting");
        await _service.ExportDistribution(distribution);
        _snackbarService.Show("Exporting", "Done!");
    }

    private async Task OnRefreshDistributions()
    {
        Distributions.Clear();
        (await _service.ListDistributions()).ToList()
            .ForEach(distribution => { Distributions.Add(distribution); });
    }
}