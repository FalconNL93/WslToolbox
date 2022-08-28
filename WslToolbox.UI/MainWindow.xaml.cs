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
    }
}