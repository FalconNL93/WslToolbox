using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Contracts.Views;

namespace WslToolbox.UI.Models;

public class ModalResult
{
    public ContentDialogResult ContentDialogResult { get; set; }
    public ModalPage? Modal { get; set; }
}