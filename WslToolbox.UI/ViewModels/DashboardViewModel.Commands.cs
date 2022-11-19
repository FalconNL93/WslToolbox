using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Views.Modals;

namespace WslToolbox.UI.ViewModels;

public partial class DashboardViewModel
{
    public bool CanRenameDistribution => false;
    public bool CanMoveDistribution => false;

    [RelayCommand]
    private async Task StartAllDistribution()
    {
        foreach (var distribution in Distributions)
        {
            await _distributionService.StartDistribution(distribution);
        }
    }

    [RelayCommand]
    private async Task StopAllDistribution()
    {
        foreach (var distribution in Distributions)
        {
            await _distributionService.StopDistribution(distribution);
        }
    }

    [RelayCommand]
    private async Task RestartAllDistribution()
    {
        foreach (var distribution in Distributions)
        {
            await _distributionService.RestartDistribution(distribution);
        }
    }

    [RelayCommand]
    private async Task AddDistribution(Page page)
    {
        var available = await _distributionService.ListInstallableDistributions();
        var distributions = available.Where(x => !x.IsInstalled).ToList();
        try
        {
            var installDistribution = await page.ShowModal<AddDistribution>(
                distributions, "Add distribution", "Add");

            if (installDistribution.Modal == null || installDistribution.ContentDialogResult != ContentDialogResult.Primary)
            {
                return;
            }

            _distributionService.InstallDistribution(installDistribution.Modal.GetSelectedItem<Distribution>());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not start {Message}", e.Message);
        }
    }

    [RelayCommand]
    private async Task RefreshDistributions()
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

    [RelayCommand]
    private async Task DeleteDistribution(Distribution? distribution)
    {
        await _distributionService.DeleteDistribution(distribution);
    }

    [RelayCommand]
    private async Task RestartDistribution(Distribution? distribution)
    {
        await _distributionService.StopDistribution(distribution);
        await _distributionService.StartDistribution(distribution);
    }

    [RelayCommand]
    private async Task StopDistribution(Distribution? distribution)
    {
        await _distributionService.StopDistribution(distribution);
    }

    [RelayCommand]
    private async Task StartDistribution(Distribution? distribution)
    {
        await _distributionService.StartDistribution(distribution);
    }

    [RelayCommand(CanExecute = nameof(CanRenameDistribution))]
    private async Task RenameDistribution(Distribution? distribution)
    {
    }

    [RelayCommand(CanExecute = nameof(CanMoveDistribution))]
    private async Task MoveDistribution(Distribution? distribution)
    {
    }
}