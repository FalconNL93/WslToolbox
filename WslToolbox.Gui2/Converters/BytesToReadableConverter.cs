using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WslToolbox.Gui2.Converters;

public class BytesToReadableConverter : MarkupExtension, IValueConverter
{
    private static BytesToReadableConverter? _converter;

    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        string[] suffixNames = {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
        var counter = 0;
        var dValue = decimal.Parse(value.ToString() ?? string.Empty);
        while (Math.Round(dValue / 1024) >= 1)
        {
            dValue /= 1024;
            counter++;
        }

        return $"{dValue:n1} {suffixNames[counter]}";
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _converter ??= new BytesToReadableConverter();
    }
}