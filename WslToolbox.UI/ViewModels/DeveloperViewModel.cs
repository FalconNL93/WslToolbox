using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Services;

namespace WslToolbox.UI.ViewModels;

public class DeveloperViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly ILogger<DeveloperViewModel> _logger;
    private readonly IMessenger _messenger;

    public DeveloperViewModel(
        ILogger<DeveloperViewModel> logger,
        DistributionService distributionService,
        IMessenger messenger
    )
    {
        _logger = logger;
        _distributionService = distributionService;
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
    }
}