using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace WslToolbox.Gui2.ViewModels;

public class SettingsViewModel : ObservableObject, INavigationAware
{
    private readonly ILogger<SettingsViewModel> _logger;
    private readonly INavigationService _navigationService;

    public SettingsViewModel(
        INavigationService navigationService,
        ILogger<SettingsViewModel> logger)
    {
        _navigationService = navigationService;
        _logger = logger;
    }


    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }
}