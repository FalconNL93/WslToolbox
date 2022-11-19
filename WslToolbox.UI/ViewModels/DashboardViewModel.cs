using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.UI.Core.Commands;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;

namespace WslToolbox.UI.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly ILogger<DashboardViewModel> _logger;

    [ObservableProperty]
    private bool _isRefreshing = true;

    public DashboardViewModel(DistributionService distributionService,
        ILogger<DashboardViewModel> logger)
    {
        _distributionService = distributionService;
        _logger = logger;

        EventHandlers();
    }

    public OpenUrlCommand OpenUrlCommand { get; } = new();

    public ObservableCollection<Distribution> Distributions { get; set; } = new();

    private void EventHandlers()
    {
        StartDistributionCommand.DistributionStartFinished += OnReloadExecution;
        TerminateDistributionCommand.DistributionTerminateFinished += OnReloadExecution;
        UnregisterDistributionCommand.DistributionUnregisterFinished += OnReloadExecution;
    }

    private void OnReloadExecution(object? sender, EventArgs e)
    {
        RefreshDistributionsCommand.Execute(null);
    }
}