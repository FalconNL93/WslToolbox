using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Views;

[ObservableObject]
public sealed partial class TrayIconView : UserControl
{
    [ObservableProperty]
    private bool _isWindowVisible;

    private bool CanOpenWindow()
    {
        return IsWindowVisible;
    }

    public TrayIconView()
    {
        InitializeComponent();
    }

    [RelayCommand(CanExecute = nameof(CanOpenWindow))]
    private void ShowHideWindow()
    {
        var window = App.MainWindow;
        if (window.Visible)
        {
            window.Hide();
        }

        IsWindowVisible = window.Visible;
    }

    [RelayCommand]
    private void ExitApplication()
    {
        App.HandleClosedEvents = false;
        TrayIcon.Dispose();
        App.MainWindow.Close();
    }
}