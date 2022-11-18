using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Messengers;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class InputDialog : ContentDialog
{
    public InputDialogModel ViewModel { get; set; }
    public InputDialog()
    {
        InitializeComponent();
    }
}