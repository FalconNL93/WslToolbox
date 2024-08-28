using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Models;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public partial class WslSettingsViewModel : ObservableRecipient
{
    private readonly WslConfigurationService _wslConfig;

    public WslSettingsViewModel(WslConfigurationService wslConfig)
    {
        _wslConfig = wslConfig;
        ReadWslSettings();
    }

    [ObservableProperty]
    private WslConfigModel _wslConfigModel = new()
    {
        Experimental = new ExperimentalSection
        {
            AutoMemoryReclaim = "initial"
        }
    };

    public readonly Dictionary<string, string> NetworkingModes = WslConfigurationService.NetworkingModeList;

    private void ReadWslSettings()
    {
        WslConfigModel = _wslConfig.GetConfig();
    }

    [RelayCommand]
    private void SaveConfiguration()
    {
        _wslConfig.WriteConfig("wsl2", "networkingMode", WslConfigModel.Root.NetworkingMode);
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