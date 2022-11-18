using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Views;

namespace WslToolbox.UI.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool _isBackEnabled;

    [ObservableProperty]
    private object? _selected;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    public INavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}