using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.ViewModels;

public partial class WslSettingsViewModel : ObservableRecipient
{
    [ObservableProperty]
    private WslConfigModel _wslConfigModel = new()
    {
        Experimental = new ExperimentalSection
        {
            AutoMemoryReclaim = "initial"
        }
    };

    public readonly Dictionary<string, string> NetworkingModes = WslConfigHelper.NetworkingModeList;

    public WslSettingsViewModel()
    {
        ReadWslSettings();
    }

    private void ReadWslSettings()
    {
        WslConfigModel = WslConfigHelper.GetConfig();
    }

    [RelayCommand]
    private void SaveConfiguration()
    {
        WslConfigHelper.WriteConfig("wsl2", "networkingMode", WslConfigModel.Root.NetworkingMode);
    }
}