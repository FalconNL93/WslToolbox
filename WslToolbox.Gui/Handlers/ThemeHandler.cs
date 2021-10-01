using System.Windows;
using ControlzEx.Theming;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public static class ThemeHandler
    {
        public static void Set(ThemeConfiguration.Styles style)
        {
            if (style == ThemeConfiguration.Styles.Auto)
            {
                ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
                ThemeManager.Current.SyncTheme();
            }
            else
            {
                _ = ThemeManager.Current.ChangeTheme(Application.Current,
                    style == ThemeConfiguration.Styles.Light
                        ? ThemeConfiguration.Light
                        : ThemeConfiguration.Dark
                );
            }
        }
    }
}