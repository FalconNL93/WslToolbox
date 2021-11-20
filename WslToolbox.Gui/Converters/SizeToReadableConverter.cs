using System;
using System.Globalization;
using System.Windows.Data;
using static System.Decimal;

namespace WslToolbox.Gui.Converters
{
    public class SizeToReadableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            string[] suffixNames = {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
            var counter = 0;
            TryParse(value.ToString(), out var dValue);
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                counter++;
            }

            return $"{dValue:n1} {suffixNames[counter]}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}