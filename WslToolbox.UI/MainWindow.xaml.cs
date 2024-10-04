using CommunityToolkit.Mvvm.Messaging;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Messengers;

namespace WslToolbox.UI;

public sealed partial class MainWindow : WindowEx
{
    public bool MinimizeToTray;
    public bool AlwaysHideIcon;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/app.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        WeakReferenceMessenger.Default.Register<UserOptionsChanged>(this, OnUserOptionsChanged);
    }

    public void ApplyUserConfiguration()
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            IsShownInSwitchers = !AlwaysHideIcon;
        });
    }

    private void OnUserOptionsChanged(object recipient, UserOptionsChanged message)
    {
        var useSystemTray = message.UserOptions.UseSystemTray;
        
        MinimizeToTray = useSystemTray && message.UserOptions.MinimizeToTray;
        AlwaysHideIcon = useSystemTray && message.UserOptions.AlwaysHideIcon;
        
        ApplyUserConfiguration();
    }

    private void OnWindowStateChanged(object? sender, WindowState e)
    {
        if (e == WindowState.Minimized)
        {
            IsShownInSwitchers = !MinimizeToTray;
            return;
        }

        IsShownInSwitchers = !AlwaysHideIcon;
    }
}