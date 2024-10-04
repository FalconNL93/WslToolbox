using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using WslToolbox.Core.Legacy.Commands.Service;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public partial class WslSettingsViewModel : ObservableRecipient
{
    private readonly ILogger<WslSettingsViewModel> _logger;
    private readonly WslConfigurationService _wslConfig;

    [ObservableProperty]
    private ObservableCollection<WslSetting> _rootCollection = [];

    public WslSettingsViewModel(WslConfigurationService wslConfig, ILogger<WslSettingsViewModel> logger)
    {
        _wslConfig = wslConfig;
        _logger = logger;
        ReadWslSettings();
    }

    private void ReadWslSettings()
    {
        var wslConfig = _wslConfig.GetConfig();
        var wsl2Section = wslConfig.Wsl2Section;

        foreach (var wslSetting in wsl2Section.Settings)
        {
            RootCollection.Add(wslSetting);
        }
    }

    [RelayCommand]
    private async Task RestartWslService()
    {
        await StopServiceCommand.Execute();
        await StartServiceCommand.Execute();
    }

    [RelayCommand]
    private void SaveConfiguration()
    {
        _logger.LogInformation("Writing WSL Configuration {@Config}", RootCollection);
        _wslConfig.WriteConfig(RootCollection);
    }

    [RelayCommand]
    private void RestoreConfiguration()
    {
        _wslConfig.RestoreConfiguration();
        ReadWslSettings();
    }

    [RelayCommand]
    private void OpenConfiguration()
    {
        try
        {
            ShellHelper.OpenFile(Toolbox.WslConfiguration);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}