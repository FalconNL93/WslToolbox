using System;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFUI.Appearance;
using WPFUI.Common;
using WPFUI.Controls;
namespace WslToolbox.Gui2.Views;

public partial class Container : Window
{
    public Container()
    {
        InitializeComponent();

        Loaded += (sender, args) =>
        {
            Watcher.Watch(this, BackgroundType.Mica, true);
        };

        RootTitleBar.CloseActionOverride = CloseActionOverride;

        Theme.Changed += ThemeOnChanged;

        Task.Run(async () =>
        {
            await Task.Delay(4000);

            Application.Current.Dispatcher.Invoke(() =>
            {
                RootWelcomeGrid.Visibility = Visibility.Hidden;
                RootGrid.Visibility = Visibility.Visible;
            });
        });
    }

    private void ThemeOnChanged(ThemeType currentTheme, Color systemAccent)
    {
        Debug.WriteLine(
            $"DEBUG | {typeof(Container)} was informed that the theme has been changed to {currentTheme}",
            "WPFUI.Demo");
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
        Debug.WriteLine("DEBUG | Root dialog action button was clicked!", "WPFUI.Demo");
    }

    private void RootDialog_RightButtonClick(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DEBUG | Root dialog custom right button was clicked!", "WPFUI.Demo");

        RootDialog.Show = false;
    }

    private void RootNavigation_OnNavigated(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DEBUG | Page now is: " + (sender as NavigationFluent)?.PageNow,
            "WPFUI.Demo");
    }

    private void TitleBar_OnMinimizeClicked(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DEBUG | Minimize button clicked", "WPFUI.Demo");
    }

    private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem) return;

        string tag = menuItem.Tag as string ?? String.Empty;

        Debug.WriteLine("DEBUG | Menu item clicked: " + tag, "WPFUI.Demo");
    }

    private void RootTitleBar_OnNotifyIconClick(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DEBUG | Notify Icon clicked", "WPFUI.Demo");
    }

    private void RootNavigation_OnNavigatedForward(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DEBUG | Navigated forward", "WPFUI.Demo");
    }

    private void RootNavigation_OnNavigatedBackward(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("DEBUG | Navigated backward", "WPFUI.Demo");
    }
}