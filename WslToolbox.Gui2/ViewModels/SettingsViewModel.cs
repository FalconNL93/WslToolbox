using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly ILogger<SettingsViewModel> _logger;

    public SettingsViewModel(
        ILogger<SettingsViewModel> logger,
        IOptions<AppConfig> options)
    {
        _logger = logger;
        Options = options.Value;
    }

    public AppConfig Options { get; }
}