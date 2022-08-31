using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Helpers;

public class NavigationHelper
{
    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationHelper), new PropertyMetadata(null));

    public static string GetNavigateTo(NavigationViewItem item)
    {
        return (string) item.GetValue(NavigateToProperty);
    }

    public static void SetNavigateTo(NavigationViewItem item, string value)
    {
        item.SetValue(NavigateToProperty, value);
    }
}