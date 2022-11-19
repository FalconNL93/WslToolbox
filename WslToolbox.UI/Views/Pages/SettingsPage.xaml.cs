using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    public SettingsViewModel ViewModel { get; }

    private async void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var theme = ThemeSelector.SelectedItem;

        if (theme is null)
        {
            return;
        }

        var selectedTheme = theme switch
        {
            "Default" => ElementTheme.Default,
            "Dark" => ElementTheme.Dark,
            "Light" => ElementTheme.Light,
            _ => ElementTheme.Default
        };

        await ViewModel.ThemeChangeCommand.ExecuteAsync(selectedTheme);
    }
}