using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace WslToolbox.Gui.Converters
{
    public class ShortcutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var keyboardShortcut = (Key) value;

            return keyboardShortcut switch
            {
                Key.OemComma => ",",
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}