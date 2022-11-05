using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public class DeveloperViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly ILogger<DeveloperViewModel> _logger;

    public DeveloperViewModel(
        ILogger<DeveloperViewModel> logger,
        DistributionService distributionService
    )
    {
        _logger = logger;
        _distributionService = distributionService;

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
        try
        {
            page.ShowProgressModal(new ProgressModel
            {
                Title = "Executing",
                Message = "Executing command, please wait...",
                ShowClose = true
            });
            page.UpdateModal("Updating distributions...");
            var dist = await _distributionService.ListDistributions();
            var first = dist.FirstOrDefault();

            var bb = await _distributionService.ExecuteCommand(first, "whoami");
            page.UpdateModal($"Output: {bb.Output}");
            await Task.Delay(TimeSpan.FromSeconds(5));

            _logger.LogInformation("Command: {Command} Output: {Output}", bb.Command, bb.Output);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error");
            throw;
        }
    }
}