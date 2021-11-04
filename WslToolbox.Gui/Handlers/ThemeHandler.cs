using System.Windows;
using ModernWpf;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public static class ThemeHandler
    {
        public static void SetTheme(this FrameworkElement element, ElementTheme style)
        {
            if (style == ElementTheme.Default)
            {
                ThemeManager.SetRequestedTheme(element, ElementTheme.Default);
            }
            else
            {
                ThemeManager.SetRequestedTheme(element, style);
            }
        }
    }
}