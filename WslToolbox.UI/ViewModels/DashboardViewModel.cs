using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;

namespace WslToolbox.UI.ViewModels;

public class DashboardViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly ILogger<DashboardViewModel> _logger;
    private readonly IConfigurationService _configurationService;

    public DashboardViewModel(DistributionService distributionService, ILogger<DashboardViewModel> logger, IConfigurationService configurationService)
    {
        _distributionService = distributionService;
        _logger = logger;
        _configurationService = configurationService;

        RefreshDistributions = new AsyncRelayCommand(OnRefreshDistributions);
    }

    public AsyncRelayCommand RefreshDistributions { get; }

    public ObservableCollection<Distribution> Distributions { get; set; } = new();

    private async Task OnRefreshDistributions()
    {
        try
        {
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
    }
}