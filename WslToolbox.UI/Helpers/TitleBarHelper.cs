using Windows.UI;
using Microsoft.UI.Xaml;

namespace WslToolbox.UI.Helpers;

public static class TitleBarHelper
{
    public static void SetTitlebarColor(this Window window, Color color)
    {
        window.ExtendsContentIntoTitleBar = false;
        window.SetTitleBar(null);
        window.AppWindow.TitleBar.ButtonForegroundColor = color;
    }

    public static void SetCaptionButtonBackgroundColors(Window window, Color? color)
    {
        var titleBar = window.AppWindow.TitleBar;
        titleBar.ButtonBackgroundColor = color;
    }

    public static void SetForegroundColor(Window window, Color? color)
    {
        var titleBar = window.AppWindow.TitleBar;
        titleBar.ForegroundColor = color;
    }

    public static void SetBackgroundColor(Window window, Color? color)
    {
        var titleBar = window.AppWindow.TitleBar;
        titleBar.BackgroundColor = color;
    }
}