using CommunityToolkit.Mvvm.ComponentModel;
using IniParser;
using IniParser.Model.Configuration;
using IniParser.Parser;
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

    public WslSettingsViewModel()
    {
        ReadWslSettings();
    }

    private void ReadWslSettings()
    {
        WslConfigModel = WslConfigHelper.GetConfig();
    }
}