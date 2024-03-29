﻿using CommunityToolkit.Mvvm.Input;

namespace WslToolbox.UI.Models;

public class SimpleDialogModel
{
    public string Message { get; set; }
    public string Title { get; set; } = App.Name;
    public string PrimaryButtonText { get; set; } = "Close";
    public string SecondaryButtonText { get; set; }
    public string TextBoxMessage { get; set; }
    public bool WithTextBoxMessage => !string.IsNullOrEmpty(TextBoxMessage);
    public IRelayCommand? PrimaryButtonCommand { get; set; }
    public IRelayCommand? SecondaryButtonCommand { get; set; }
}