﻿using CommunityToolkit.Mvvm.ComponentModel;
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

    public SettingsViewModel(
        ILogger<SettingsViewModel> logger,
        IOptions<AppConfig> options
    )
    {
        _logger = logger;
        AppConfig = options.Value;
        ModifiedAppConfig = options.Value.Clone();

        SaveConfiguration = new RelayCommand(() =>
        {
            AppConfig = ModifiedAppConfig;
            options.Save(ModifiedAppConfig);
        });

        RevertConfiguration = new RelayCommand(() => { AppConfig = new AppConfig(); });
    }

    public RelayCommand SaveConfiguration { get; }
    public RelayCommand RevertConfiguration { get; }

    private AppConfig AppConfig
    {
        set => SetProperty(ref _appConfig, value);
    }

    public AppConfig ModifiedAppConfig { get; }
}