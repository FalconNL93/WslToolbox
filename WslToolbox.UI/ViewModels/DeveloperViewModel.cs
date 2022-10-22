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
    private readonly ILogger<DeveloperViewModel> _logger;
    private readonly DistributionService _distributionService;

    public DeveloperViewModel(
        ILogger<DeveloperViewModel> logger,
        DistributionService distributionService
    )
    {
        _logger = logger;
        _distributionService = distributionService;
        TestWindow = new RelayCommand<Page>(OnTestWindow);
    }

    public RelayCommand<Page> TestWindow { get; }

    private async void OnTestWindow(Page? page)
    {
        try
        {
            page.ShowProgressModal(new ProgressModel
            {
                Title = "Executing",
                Message = "Executing command, please wait..."
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