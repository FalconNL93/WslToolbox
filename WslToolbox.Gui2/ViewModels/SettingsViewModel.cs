using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.Gui2.Extensions;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly ILogger<SettingsViewModel> _logger;
    private AppConfig? _appConfig;
    public RelayCommand SaveConfiguration { get; }
    public RelayCommand RevertConfiguration { get; }

    public AppConfig AppConfig
    {
        get => _appConfig ?? new AppConfig();
        private set => SetProperty(ref _appConfig, value);
    }

    public SettingsViewModel(
        ILogger<SettingsViewModel> logger,
        IOptions<AppConfig> options)
    {
        _logger = logger;
        AppConfig = options.Value;

        SaveConfiguration = new RelayCommand(options.Save);
        RevertConfiguration = new RelayCommand(() => { AppConfig = new AppConfig(); });
    }
}