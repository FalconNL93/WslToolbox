using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Core.Commands.Service;

namespace WslToolbox.Gui2.ViewModels;

public class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly ILogger<DashboardViewModel> _logger;
    public ICommand StopDistribution => new RelayCommand<string>(OnStopDistribution);

    public DashboardViewModel(INavigationService navigationService, ILogger<DashboardViewModel> logger)
    {
        _navigationService = navigationService;
        _logger = logger;
    }

    private async void OnStopDistribution(string? parameter)
    {
        var list = await ListServiceCommand.ListDistributions();
        
        foreach (var dist in list)
        {
            Debug.WriteLine(dist.Name);
        }
    }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }
}