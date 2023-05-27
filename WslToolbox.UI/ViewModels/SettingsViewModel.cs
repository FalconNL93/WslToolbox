using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Versioning;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Args;
using WslToolbox.UI.Core.Commands;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Models;
using WslToolbox.UI.Notifications;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IConfigurationService _configurationService;
    private readonly DownloadService _downloadService;
    private readonly ILogger<SettingsViewModel> _logger;
    private readonly IMessenger _messenger;
    private readonly AppNotificationService _notificationService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly UpdateService _updateService;
    public readonly string AppDescription = $"{App.Name} {Toolbox.Version} ({Toolbox.ProcessType})";

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private bool _isPackage;

    [ObservableProperty]
    private DownloadUpdateModel _updateModel = new();

    [ObservableProperty]
    private UpdateResultModel _updaterResult = new();

    [ObservableProperty]
    private bool _updateServiceAvailable;

    public SettingsViewModel(IThemeSelectorService themeSelectorService,
        IConfigurationService configurationService,
        IOptions<UserOptions> userOptions,
        IOptions<NotificationOptions> notificationOptions,
        AppNotificationService notificationService,
        UpdateService updateService,
        IMessenger messenger,
        ILogger<SettingsViewModel> logger,
        DownloadService downloadService
    )
    {
        _themeSelectorService = themeSelectorService;
        _configurationService = configurationService;
        _notificationService = notificationService;
        _updateService = updateService;
        _messenger = messenger;
        _logger = logger;
        _downloadService = downloadService;
        _elementTheme = _themeSelectorService.Theme;

        NotificationOptions = notificationOptions.Value;
        UserOptions = userOptions.Value;

        _updateServiceAvailable = !App.IsPackage();
        _isPackage = App.IsPackage();

        DownloadService.ProgressChanged += DownloadServiceOnProgressChanged;
        DownloadUpdateEvent += (_, _) => { DownloadUpdateCommand.Execute(this); };

        var frameworkDescription = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName;
        AppDescription = $"{AppDescription}{Environment.NewLine}{frameworkDescription}";
    }

    public UserOptions UserOptions { get; }
    public NotificationOptions NotificationOptions { get; }
    public OpenUrlCommand OpenUrlCommand { get; } = new();

    public ObservableCollection<string> Themes { get; set; } = new(Enum.GetNames(typeof(ElementTheme)));

    public static event EventHandler DownloadUpdateEvent;

    private void DownloadServiceOnProgressChanged(object? sender, UserProgressChangedEventArgs e)
    {
        _messenger.ShowUpdateInfoBar($"Downloading file {e.TotalBytesDownloadedHuman} of {e.TotalBytesHuman}...");
    }

    [RelayCommand]
    private async Task CheckForUpdates()
    {
        UpdaterResult = new UpdateResultModel { IsChecking = true };

        await Task.Delay(TimeSpan.FromSeconds(2));
        UpdaterResult = await _updateService.GetUpdateDetails();

        if (UpdaterResult.HasError)
        {
            _messenger.ShowUpdateInfoBar("Could not retrieve update information", "Error", InfoBarSeverity.Error);
        }
        else if (UpdaterResult.UpdateAvailable)
        {
            _messenger.ShowUpdateInfoBar("A new update is available", "Update available", InfoBarSeverity.Success);
            _notificationService.Show(UpdateNotification.UpdatesAvailable(UpdaterResult));
            var result = await _messenger.ShowUpdateDialog(new UpdateViewModel
            {
                EnableInstallUpdate = true,
                CurrentVersion = UpdaterResult.CurrentVersion,
                LatestVersion = UpdaterResult.LatestVersion,
                ReleaseNotes = string.Empty
            });

            if (result == ContentDialogResult.Primary)
            {
                await DownloadUpdate();
            }
        }
        else
        {
            _messenger.ShowUpdateInfoBar("You are running the latest version", "No new updates", InfoBarSeverity.Success);
            _notificationService.Show(UpdateNotification.NoUpdatesNotification);
        }
    }

    [RelayCommand]
    private async Task OpenAppInStore()
    {
        ShellHelper.OpenFile(Toolbox.StoreUrl);
    }

    [RelayCommand]
    private async Task DownloadUpdate()
    {
        if (Toolbox.GetAppType() == Toolbox.AppTypes.Setup || Toolbox.GetAppType() == Toolbox.AppTypes.Portable)
        {
            _messenger.ShowUpdateInfoBar("Downloading update file...");
            try
            {
                var downloadedFile = await _downloadService.DownloadFileAsync(UpdaterResult);
                _messenger.ShowUpdateInfoBar("Starting updater...");
                ShellHelper.OpenExecutable(downloadedFile,
                    new List<string>
                    {
                        "/SILENT"
                    },
                    true);
            }
            catch (Exception e)
            {
                _messenger.ShowUpdateInfoBar("Could not download update file", severity: InfoBarSeverity.Error);
                _logger.LogError(e, "Could not download update file");
            }

            return;
        }

        ShellHelper.OpenUrl(UpdaterResult.DownloadUri);
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

    public static void OnDownloadUpdateEvent()
    {
        DownloadUpdateEvent?.Invoke(null, EventArgs.Empty);
    }
}