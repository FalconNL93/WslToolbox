using Microsoft.UI.Xaml;
using WslToolbox.UI.Helpers;

namespace WslToolbox.UI;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/app.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
        
        TrayIcon.Visibility = Visibility.Visible;
        
        // TrayIcon.ForceCreate();
        //
        // TrayIcon.ShowNotification("bla", "bla");
    }
}