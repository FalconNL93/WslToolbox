using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace WslToolbox.UI.Converters;

public class DateTimeToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (targetType != typeof(Visibility))
        {
            throw new InvalidOperationException($"The target must be a {typeof(Visibility)}");
        }

        return (DateTime) value == DateTime.MinValue ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new ArgumentException("Not supported");
    }
}