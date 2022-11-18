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

namespace WslToolbox.UI.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IConfigurationService _configurationService;
    private readonly IMessenger _messenger;
    private readonly IAppNotificationService _notificationService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly UpdateService _updateService;
    private readonly UserOptions _userOptions;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    private UpdateResultModel _updaterResult = new();

    public SettingsViewModel(IThemeSelectorService themeSelectorService,
        IOptions<UserOptions> userOptions,
        IConfigurationService configurationService,
        UpdateService updateService,
        IMessenger messenger,
        IAppNotificationService notificationService
    )
    {
        _themeSelectorService = themeSelectorService;
        _configurationService = configurationService;
        _updateService = updateService;
        _messenger = messenger;
        _notificationService = notificationService;
        _elementTheme = _themeSelectorService.Theme;
        UserOptions = userOptions.Value;
    }

    public UpdateResultModel UpdaterResult
    {
        get => _updaterResult;
        set => SetProperty(ref _updaterResult, value);
    }

    public UserOptions UserOptions { get; }

    public ObservableCollection<string> Themes { get; set; } = new(Enum.GetNames(typeof(ElementTheme)));

    [RelayCommand]
    private async Task CheckForUpdates()
    {
        UpdaterResult = new UpdateResultModel {UpdateStatus = "Checking for updates..."};
        UpdaterResult = await _updateService.GetUpdateDetails();

        _notificationService.Show("test");

        await Task.Delay(TimeSpan.FromSeconds(10));
    }

    [RelayCommand]
    private async Task ThemeChange(ElementTheme param)
    {
        if (ElementTheme == param)
        {
            return;
        }

        ElementTheme = param;
        await _themeSelectorService.SetThemeAsync(param);
    }

    [RelayCommand]
    private static void OpenLogFile()
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

    [RelayCommand]
    private static void OpenAppDir()
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

    [RelayCommand]
    private void SaveConfiguration()
    {
        _configurationService.Save(UserOptions);
    }

    [RelayCommand]
    private void RestoreDefaultConfiguration()
    {
        _configurationService.Restore<UserOptions>();

        RestoreDefaultConfigurationCommand.NotifyCanExecuteChanged();
        OpenConfigurationCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private static void OpenConfiguration()
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