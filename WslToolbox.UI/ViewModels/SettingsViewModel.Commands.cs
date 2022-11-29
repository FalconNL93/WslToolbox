using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Notifications;

namespace WslToolbox.UI.ViewModels;

public partial class SettingsViewModel
{
    [ObservableProperty]
    private ElementTheme _elementTheme;

    [RelayCommand]
    private async Task CheckForUpdates()
    {
        UpdaterResult = new UpdateResultModel {UpdateStatus = "Checking for updates..."};
        UpdaterResult = await _updateService.GetUpdateDetails();

        if (!UpdaterResult.UpdateAvailable)
        {
            _appNotificationService.Show(UpdateNotification.NoUpdates);
            _messenger.ShowInfoBar("No updates", "No updates available");
        }
        else
        {
            await _messenger.ShowUpdateDialog(new UpdateViewModel
            {
                EnableInstallUpdate = true,
            });
        }

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