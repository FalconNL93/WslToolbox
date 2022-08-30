using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IConfigurationService _configurationService;
    private readonly UserOptions _userOptions;
    private ElementTheme _elementTheme;

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IOptions<UserOptions> userOptions, IConfigurationService configurationService)
    {
        _themeSelectorService = themeSelectorService;
        _configurationService = configurationService;
        _elementTheme = _themeSelectorService.Theme;
        UserOptions = userOptions.Value;

        SwitchThemeCommand = new RelayCommand<ElementTheme>(OnThemeChange);
        SaveConfiguration = new RelayCommand(OnSaveConfiguration);
    }

    private void OnSaveConfiguration()
    {
        _configurationService.Save(UserOptions);
    }

    public UserOptions UserOptions { get; }

    public ObservableCollection<string> Themes { get; set; } = new(Enum.GetNames(typeof(ElementTheme)));

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set
        {
            SetProperty(ref _elementTheme, value);
            SwitchThemeCommand.Execute(value);
        }
    }

    public ICommand SwitchThemeCommand { get; }
    public ICommand SaveConfiguration { get; }

    private async void OnThemeChange(ElementTheme param)
    {
        if (ElementTheme == param)
        {
            return;
        }

        ElementTheme = param;
        await _themeSelectorService.SetThemeAsync(param);
    }
}