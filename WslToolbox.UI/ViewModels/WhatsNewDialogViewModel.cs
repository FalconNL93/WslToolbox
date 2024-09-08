﻿using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.ViewModels;

public partial class WhatsNewDialogViewModel
{
    private readonly IConfigurationService _configurationService;
    public readonly UserOptions UserOptions;

    public WhatsNewDialogViewModel(IConfigurationService configurationService,
        IOptions<UserOptions> userOptions
    )
    {
        _configurationService = configurationService;
        UserOptions = userOptions.Value;
    }

    public string Message { get; set; }
    public string Title { get; set; } = App.Name;
    public string PrimaryButtonText { get; set; } = "Close";
    public string SecondaryButtonText { get; set; }

    [RelayCommand]
    private void SaveConfiguration()
    {
    }
}