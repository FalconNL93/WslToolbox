using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace WslToolbox.UI.Helpers;

public static class ControlHelper
{
    public static T? FindVisualChild<T>(DependencyObject visual) where T : DependencyObject
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
        {
            var child = VisualTreeHelper.GetChild(visual, i);
            if (child == null)
            {
                continue;
            }

            var correctlyTyped = child as T;
            if (correctlyTyped != null)
            {
                return correctlyTyped;
            }

            var descendent = FindVisualChild<T>(child);
            if (descendent != null)
            {
                return descendent;
            }
        }

        return null;
    }
}