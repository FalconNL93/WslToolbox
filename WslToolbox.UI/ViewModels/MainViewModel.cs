using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;

namespace WslToolbox.UI.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;

    public MainViewModel(DistributionService distributionService)
    {
        _distributionService = distributionService;

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