using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace WslToolbox.UI.Helpers;

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (targetType != typeof(bool))
        {
            throw new InvalidOperationException("The target must be a boolean");
        }

        return !(bool) value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new ArgumentException("Not supported");
    }
}