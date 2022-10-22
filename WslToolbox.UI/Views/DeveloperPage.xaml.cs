using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views;

public sealed partial class DeveloperPage : Page
{
    public DeveloperPage()
    {
        ViewModel = App.GetService<DeveloperViewModel>();
        InitializeComponent();
    }

    public DeveloperViewModel ViewModel { get; }
}