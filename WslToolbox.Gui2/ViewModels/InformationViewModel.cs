using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace WslToolbox.Gui2.ViewModels;

public class InformationViewModel : ObservableObject
{
    private readonly ILogger<InformationViewModel> _logger;

    public InformationViewModel(ILogger<InformationViewModel> logger)
    {
        _logger = logger;
    }

    public string? AppVersion { get; } = App.AssemblyVersionFull;
}