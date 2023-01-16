using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class DeveloperPage : Page
{
    public DeveloperPage()
    {
        ViewModel = App.GetService<DeveloperViewModel>();
        InitializeComponent();
        
        WeakReferenceMessenger.Default.Register<ProgressChangedMessage>(this, OnShowInfoBar);
    }

    private void OnShowInfoBar(object recipient, ProgressChangedMessage message)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
        {
            PageBarText.Maximum = message.Value.TotalBytes;
            PageBarText.Value = message.Value.Progress; 
        });
    }

    public DeveloperViewModel ViewModel { get; }

    private void OnFakeUpdateSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = FakeUpdateResultsSelector.SelectedItem;
        ViewModel.DevOptions.Value.FakeUpdateResult = selectedItem switch
        {
            nameof(FakeUpdateResult.UpdateAvailable) => FakeUpdateResult.UpdateAvailable,
            nameof(FakeUpdateResult.NoUpdate) => FakeUpdateResult.NoUpdate,
            _ => FakeUpdateResult.Off,
        };
    }
}