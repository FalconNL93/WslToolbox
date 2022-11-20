using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.Notifications;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public class DeveloperViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly IAppNotificationService _appNotificationService;
    private readonly IMessenger _messenger;
    private readonly ILogger<DeveloperViewModel> _logger;

    public DeveloperViewModel(
        ILogger<DeveloperViewModel> logger,
        DistributionService distributionService,
        IAppNotificationService appNotificationService,
        IMessenger messenger
    )
    {
        _logger = logger;
        _distributionService = distributionService;
        _appNotificationService = appNotificationService;
        _messenger = messenger;

        TestWindow = new RelayCommand<Page>(OnTestWindow);
        Load = new AsyncRelayCommand(OnLoad);
    }


    public RelayCommand<Page> TestWindow { get; }
    public AsyncRelayCommand Load { get; }

    private async Task OnLoad()
    {
        Debug.WriteLine("Executed");
        await Task.Delay(TimeSpan.FromSeconds(10));
        Debug.WriteLine("Finished");
    }

    private async void OnTestWindow(Page? page)
    {
        var dialog = await _messenger.ShowInputDialog(new InputDialogModel
        {
            Message = "Enter input",
            Title = "Input",
            InputFieldText = ""
        });
    }
}