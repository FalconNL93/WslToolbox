using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IAppNotificationService _appNotificationService;
    private readonly IConfigurationService _configurationService;
    private readonly IMessenger _messenger;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly UpdateService _updateService;

    [ObservableProperty]
    private UpdateResultModel _updaterResult = new();

    [ObservableProperty]
    private bool _updateServiceAvailable;

    [ObservableProperty]
    private bool _isPackage;

    public SettingsViewModel(IThemeSelectorService themeSelectorService,
        IOptions<UserOptions> userOptions,
        IOptions<RunOptions> runOptions,
        IConfigurationService configurationService,
        UpdateService updateService,
        IMessenger messenger,
        IAppNotificationService appNotificationService
    )
    {
        _themeSelectorService = themeSelectorService;
        _configurationService = configurationService;
        _updateService = updateService;
        _messenger = messenger;
        _appNotificationService = appNotificationService;
        _elementTheme = _themeSelectorService.Theme;
        UserOptions = userOptions.Value;
        RunOptions = runOptions.Value;

        _updateServiceAvailable = !App.IsPackage();
        _isPackage = App.IsPackage();
    }

    public UserOptions UserOptions { get; }
    public RunOptions RunOptions { get; }

    public ObservableCollection<string> Themes { get; set; } = new(Enum.GetNames(typeof(ElementTheme)));
}