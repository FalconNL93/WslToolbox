using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Helpers;

namespace WslToolbox.UI.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IConfigurationService _configurationService;
    private readonly IThemeSelectorService _themeSelectorService;
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
        RestoreDefaultConfiguration = new RelayCommand(OnRestoreDefaultConfiguration, () => File.Exists($"{App.AppDirectory}\\{App.UserConfiguration}"));
        OpenConfiguration = new RelayCommand(OnOpenConfiguration, () => File.Exists($"{App.AppDirectory}\\{App.UserConfiguration}"));
        OpenLogFile = new RelayCommand(OnOpenLogFile, () => File.Exists($"{App.AppDirectory}\\{App.LogFile}"));
    }

    public string? Version { get; set; } = App.Version;
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
    public RelayCommand SaveConfiguration { get; }
    public RelayCommand RestoreDefaultConfiguration { get; }
    public RelayCommand OpenConfiguration { get; }
    public RelayCommand OpenLogFile { get; }

    private static void OnOpenLogFile()
    {
        try
        {
            ShellHelper.OpenFile($"{App.AppDirectory}\\{App.LogFile}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void OnSaveConfiguration()
    {
        _configurationService.Save(UserOptions);
    }

    private void OnRestoreDefaultConfiguration()
    {
        _configurationService.Restore<UserOptions>();

        RestoreDefaultConfiguration.NotifyCanExecuteChanged();
        OpenConfiguration.NotifyCanExecuteChanged();
    }

    private async void OnThemeChange(ElementTheme param)
    {
        if (ElementTheme == param)
        {
            return;
        }

        ElementTheme = param;
        await _themeSelectorService.SetThemeAsync(param);
    }

    private static void OnOpenConfiguration()
    {
        try
        {
            ShellHelper.OpenFile($"{App.AppDirectory}\\{App.UserConfiguration}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}