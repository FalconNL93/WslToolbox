using System;
using System.Globalization;
using System.Windows.Data;
using WslToolbox.Gui.Properties;

namespace WslToolbox.Gui.Converters
{
    public class BoolToValueConverter : IValueConverter
    {
        public string TrueValue { get; set; } = Resources.BOOL_YES;
        public string FalseValue { get; set; } = Resources.BOOL_NO;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? FalseValue : (bool) value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}