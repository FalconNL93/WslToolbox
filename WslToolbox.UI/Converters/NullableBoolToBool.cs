using Microsoft.UI.Xaml.Data;

namespace WslToolbox.UI.Converters;

public class NullableBoolToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        return value ?? false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return false;
    }
}