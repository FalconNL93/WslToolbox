using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Notifications;

namespace WslToolbox.UI.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IConfigurationService _configurationService;
    private readonly IMessenger _messenger;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly UpdateService _updateService;
    public readonly string AppDescription = $"{App.Name} {Toolbox.Version} ({Toolbox.ProcessType})";

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private bool _isPackage;

    [ObservableProperty]
    private UpdateResultModel _updaterResult = new();

    [ObservableProperty]
    private bool _updateServiceAvailable;

    public SettingsViewModel(IThemeSelectorService themeSelectorService,
        IOptions<UserOptions> userOptions,
        IOptions<NotificationOptions> notificationOptions,
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
        
        NotificationOptions = notificationOptions.Value;
        UserOptions = userOptions.Value;

        _updateServiceAvailable = !App.IsPackage();
        _isPackage = App.IsPackage();
    }

    public UserOptions UserOptions { get; }
    public NotificationOptions NotificationOptions { get; }

    public ObservableCollection<string> Themes { get; set; } = new(Enum.GetNames(typeof(ElementTheme)));

    [RelayCommand]
    private async Task CheckForUpdates()
    {
        UpdaterResult = new UpdateResultModel {IsChecking = true};

        await Task.Delay(TimeSpan.FromSeconds(2));
        UpdaterResult = await _updateService.GetUpdateDetails();

        if (UpdaterResult.UpdateAvailable)
        {
            _messenger.ShowUpdateInfoBar("Update available", "A new update is available", InfoBarSeverity.Success);
            UpdateNotification.ShowUpdatesAvailableNotification(UpdaterResult);
            var result = await _messenger.ShowUpdateDialog(new UpdateViewModel
            {
                EnableInstallUpdate = true,
                CurrentVersion = UpdaterResult.CurrentVersion,
                LatestVersion = UpdaterResult.LatestVersion
            });

            if (result == ContentDialogResult.Primary)
            {
                ShellHelper.OpenUrl(UpdaterResult.DownloadUri);
            }
        }
        else
        {
            _messenger.ShowUpdateInfoBar("No new updates", "You are running the latest version", InfoBarSeverity.Success);
            UpdateNotification.ShowNoUpdatesNotification();
        }
    }

    [RelayCommand]
    private async Task OpenAppInStore()
    {
        ShellHelper.OpenFile(Toolbox.StoreUrl);
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

    [RelayCommand(CanExecute = nameof(CanOpenLogFile))]
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

    private static bool CanOpenLogFile()
    {
        return File.Exists(Toolbox.LogFile);
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