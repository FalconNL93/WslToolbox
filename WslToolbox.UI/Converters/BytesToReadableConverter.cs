using Microsoft.UI.Xaml.Data;

namespace WslToolbox.UI.Converters;

public class BytesToReadableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
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

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}