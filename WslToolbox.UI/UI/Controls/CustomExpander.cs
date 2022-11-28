using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using WslToolbox.UI.Helpers;

namespace WslToolbox.UI.UI.Controls;

public class CustomExpander : Expander
{
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var expander = GetTemplateChild("ExpanderHeader") as ToggleButton;
        if (expander != null)
        {
            expander.Loaded += TbLoaded;
        }
    }

    private static void TbLoaded(object sender, RoutedEventArgs e)
    {
        var expander = (ToggleButton) sender;
        expander.Padding = new Thickness(0, 0, 16, 0);
        expander.Loaded -= TbLoaded;
        var expanderContent = ControlHelper.FindVisualChild<ContentPresenter>(expander);
        var expanderButton = ControlHelper.FindVisualChild<Border>(expander);
        expanderContent.Margin = new Thickness(5, 0, 5, 0);
        expanderButton.Visibility = Visibility.Collapsed;

        Grid.SetColumn(expanderContent, 1);
        Grid.SetColumn(expanderButton, 0);
    }
}