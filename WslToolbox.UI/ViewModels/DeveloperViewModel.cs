using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.ViewModels;

public class DeveloperViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly ILogger<DeveloperViewModel> _logger;
    private readonly IMessenger _messenger;
    public readonly IOptions<DevOptions> DevOptions;

    public ObservableCollection<string> FakeUpdateResults { get; set; } = new(Enum.GetNames(typeof(FakeUpdateResult)));

    public DeveloperViewModel(
        ILogger<DeveloperViewModel> logger,
        DistributionService distributionService,
        IMessenger messenger,
        IOptions<DevOptions> devOptions)
    {
        _logger = logger;
        _distributionService = distributionService;
        _messenger = messenger;
        DevOptions = devOptions;
    }
}