using System.Windows;

namespace WslToolbox.Gui.Helpers
{
    public static class ResourceHelper
    {
        public static Style FindResource(object key)
        {
            return Application.Current.Resources.Contains(key) ? (Style) Application.Current.Resources[key] : null;
        }
    }
}