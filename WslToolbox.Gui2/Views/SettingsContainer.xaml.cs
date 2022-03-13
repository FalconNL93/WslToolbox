using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFUI.Appearance;
using WPFUI.Controls;

namespace WslToolbox.Gui2.Views;

public partial class SettingsContainer : Window
{
    public SettingsContainer()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            Watcher.Watch(this);
        };

        RootTitleBar.CloseActionOverride = CloseActionOverride;

        Theme.Changed += ThemeOnChanged;
    }

    private void ThemeOnChanged(ThemeType currentTheme, Color systemAccent)
    {
    }

    private void CloseActionOverride(TitleBar titleBar, Window window)
    {
        Application.Current.Shutdown();
    }

    private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
    {
        RootNavigation.Navigate("dashboard");
    }

    private void RootDialog_LeftButtonClick(object sender, RoutedEventArgs e)
    {
    }

    private void RootDialog_RightButtonClick(object sender, RoutedEventArgs e)
    {
        RootDialog.Show = false;
    }

    private void RootNavigation_OnNavigated(object sender, RoutedEventArgs e)
    {
    }

    private void TitleBar_OnMinimizeClicked(object sender, RoutedEventArgs e)
    {
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem) return;

        var tag = menuItem.Tag as string ?? string.Empty;
    }

    private void RootTitleBar_OnNotifyIconClick(object sender, RoutedEventArgs e)
    {
    }

    private void RootNavigation_OnNavigatedForward(object sender, RoutedEventArgs e)
    {
    }

    private void RootNavigation_OnNavigatedBackward(object sender, RoutedEventArgs e)
    {
    }
}