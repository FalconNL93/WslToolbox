using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;
using WslToolbox.UI.Views.Modals;

namespace WslToolbox.UI.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

        ViewModel.RefreshDistributions.Execute(null);

        WeakReferenceMessenger.Default.Register<InputDialogRequestMessage>(this, (r, m) =>
        {
            async Task<ContentDialogResult> ReceiveAsync()
            {
                var dialog = new InputDialog
                {
                    ViewModel = m.InputDialogModel,
                    XamlRoot = XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    DefaultButton = ContentDialogButton.Primary,
                };

                return await dialog.ShowAsync();
            }
    
            m.Reply(ReceiveAsync());
        });
        
    }
    

    public DashboardViewModel ViewModel { get; }
}