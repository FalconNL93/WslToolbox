using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using WslToolbox.UI.ViewModels;
using RoutedEventArgs = Microsoft.UI.Xaml.RoutedEventArgs;
using UIElement = Microsoft.UI.Xaml.UIElement;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

        ViewModel.RefreshDistributionsCommand.Execute(null);
    }

    public DashboardViewModel ViewModel { get; }
    
    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        ViewModel.ShowStartupDialogCommand.Execute(null);
    }

    private void Distribution_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
    {
        var element = (FrameworkElement) sender;
        args.TryGetPosition(sender, out var position);
        
        DistributionContextMenu.ShowAt(element, new FlyoutShowOptions
        {
            Position = position,
            ShowMode = FlyoutShowMode.Auto
        });
    }
}