using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Contracts.Views;

public abstract class ModalPage : Page
{
    public object SelectedItem { get; set; } = new();
}