using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Helpers;

namespace WslToolbox.UI.ViewModels;

public class DashboardViewModel : ObservableRecipient
{
    private readonly IConfigurationService _configurationService;
    private readonly DistributionService _distributionService;
    private readonly ILogger<DashboardViewModel> _logger;
    private bool _isRefreshing = true;

    public DashboardViewModel(DistributionService distributionService, ILogger<DashboardViewModel> logger, IConfigurationService configurationService)
    {
        _distributionService = distributionService;
        _logger = logger;
        _configurationService = configurationService;

        RefreshDistributions = new AsyncRelayCommand(OnRefreshDistributions);
        StartDistribution = new AsyncRelayCommand<Distribution>(OnStartDistribution, DistributionCommand.CanStartDistribution);
        StopDistributions = new AsyncRelayCommand<Distribution>(OnStopDistribution, DistributionCommand.CanStopDistribution);
        RestartDistribution = new AsyncRelayCommand<Distribution>(OnRestartDistribution, DistributionCommand.CanRestartDistribution);
        DeleteDistribution = new AsyncRelayCommand<Distribution>(OnDeleteDistribution);

        EventHandlers();
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public AsyncRelayCommand RefreshDistributions { get; }
    public AsyncRelayCommand<Distribution> StartDistribution { get; }
    public AsyncRelayCommand<Distribution> StopDistributions { get; }
    public AsyncRelayCommand<Distribution> RestartDistribution { get; }
    public AsyncRelayCommand<Distribution> DeleteDistribution { get; }
    public ObservableCollection<Distribution> Distributions { get; set; } = new();

    private void EventHandlers()
    {
        StartDistributionCommand.DistributionStartFinished += OnReloadExecution;
        TerminateDistributionCommand.DistributionTerminateFinished += OnReloadExecution;
    }

    private void OnReloadExecution(object? sender, EventArgs e)
    {
        RefreshDistributions.Execute(null);
    }

    private async Task OnRefreshDistributions()
    {
        _logger.LogInformation("Refreshing list of distributions");
        try
        {
            IsRefreshing = true;
            Distributions.Clear();
            (await _distributionService.ListDistributions()).ToList()
                .ForEach(distribution =>
                {
                    Distributions.Add(distribution);
                });
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private async Task OnDeleteDistribution(Distribution? distribution)
    {
        await _distributionService.DeleteDistribution(distribution);
    }

    private async Task OnRestartDistribution(Distribution? distribution)
    {
        await _distributionService.StopDistribution(distribution);
        await _distributionService.StartDistribution(distribution);
    }

    private async Task OnStopDistribution(Distribution? distribution)
    {
        await _distributionService.StopDistribution(distribution);
    }

    private async Task OnStartDistribution(Distribution? distribution)
    {
        await _distributionService.StartDistribution(distribution);
    }
}