using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Helpers;

namespace WslToolbox.UI.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IConfigurationService _configurationService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly UpdateService _updateService;
    private readonly IMessenger _messenger;
    private readonly UserOptions _userOptions;
    private ElementTheme _elementTheme;
    private UpdateResultModel _updaterResult = new();

    public SettingsViewModel(IThemeSelectorService themeSelectorService,
        IOptions<UserOptions> userOptions,
        IConfigurationService configurationService,
        UpdateService updateService,
        IMessenger messenger
    )
    {
        _themeSelectorService = themeSelectorService;
        _configurationService = configurationService;
        _updateService = updateService;
        _messenger = messenger;
        _elementTheme = _themeSelectorService.Theme;
        UserOptions = userOptions.Value;

        SwitchThemeCommand = new AsyncRelayCommand<ElementTheme>(OnThemeChange);
        SaveConfiguration = new RelayCommand(OnSaveConfiguration);
        RestoreDefaultConfiguration = new RelayCommand(OnRestoreDefaultConfiguration, () => File.Exists(Toolbox.UserConfiguration));
        OpenConfiguration = new RelayCommand(OnOpenConfiguration, () => File.Exists(Toolbox.UserConfiguration));
        OpenLogFile = new RelayCommand(OnOpenLogFile, () => File.Exists(Toolbox.UserConfiguration));
        OpenAppDir = new RelayCommand(OnOpenAppDir, () => Directory.Exists(Toolbox.AppDirectory));
        CheckForUpdates = new AsyncRelayCommand(OnCheckForUpdates);
    }

    public UpdateResultModel UpdaterResult
    {
        get => _updaterResult;
        set => SetProperty(ref _updaterResult, value);
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

    public AsyncRelayCommand<ElementTheme> SwitchThemeCommand { get; }
    public RelayCommand SaveConfiguration { get; }
    public RelayCommand RestoreDefaultConfiguration { get; }
    public RelayCommand OpenConfiguration { get; }
    public RelayCommand OpenLogFile { get; }
    public RelayCommand OpenAppDir { get; }
    public AsyncRelayCommand CheckForUpdates { get; }

    private async Task OnCheckForUpdates()
    {
        UpdaterResult = new UpdateResultModel {UpdateStatus = "Checking for updates..."};
        UpdaterResult = await _updateService.GetUpdateDetails();

        await Task.Delay(TimeSpan.FromSeconds(10));
    }

    private async Task OnThemeChange(ElementTheme param)
    {
        if (ElementTheme == param)
        {
            return;
        }

        ElementTheme = param;
        await _themeSelectorService.SetThemeAsync(param);
    }

    private static void OnOpenLogFile()
    {
        try
        {
            ShellHelper.OpenFile(Toolbox.LogFile);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private static void OnOpenAppDir()
    {
        try
        {
            ShellHelper.OpenFile(Toolbox.AppDirectory);
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

    private static void OnOpenConfiguration()
    {
        try
        {
            ShellHelper.OpenFile(Toolbox.UserConfiguration);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}