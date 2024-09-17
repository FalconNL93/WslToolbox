using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

        ViewModel.RefreshDistributionsCommand.Execute(null);
        WeakReferenceMessenger.Default.Register<InfoBarChangedMessage>(ViewModel, OnShowInfoBar);
    }

    public DashboardViewModel ViewModel { get; }

    private void OnShowInfoBar(object recipient, InfoBarChangedMessage message)
    {
        var infoBarModel = message.Value;
        InfoBar.IsOpen = infoBarModel.IsOpen;
        InfoBar.IsClosable = infoBarModel.IsClosable;
        InfoBar.IsIconVisible = infoBarModel.IsIconVisible;
        InfoBar.Severity = infoBarModel.Severity;
        InfoBar.Title = infoBarModel.Title;
        InfoBar.Message = infoBarModel.Message;
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        ViewModel.ShowStartupDialogCommand.Execute(null);
        ViewModel.CheckForUpdatesCommand.Execute(null);
    }
}