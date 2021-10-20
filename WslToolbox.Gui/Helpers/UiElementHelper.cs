using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace WslToolbox.Gui.Helpers
{
    public static class UiElementHelper
    {
        public static CheckBox AddCheckBox(string name, string content, string bind, object source,
            string requires = null)
        {
            var checkBox = new CheckBox
            {
                Name = name,
                Content = content
            };

            checkBox.SetBinding(ToggleButton.IsCheckedProperty, BindHelper.BindingObject(bind, source));

            if (requires != null)
                checkBox.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));

            return checkBox;
        }

        public static ComboBox AddComboBox(string name, IEnumerable items, string bind, object source,
            string requires = null)
        {
            var comboBox = new ComboBox
            {
                Name = name,
                ItemsSource = items
            };

            if (requires != null)
                comboBox.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));

            comboBox.SetBinding(Selector.SelectedItemProperty, BindHelper.BindingObject(bind, source));

            return comboBox;
        }

        public static TextBlock AddHyperlink(string url, string name = null, string tooltip = null,
            string bind = null)
        {
            var textBlock = new TextBlock
            {
                Padding = new Thickness(5, 0, 0, 10)
            };

            var textBlockHyperlink = new TextBlock
            {
                MaxWidth = 400,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                Text = name ?? url
            };

            var hyperlink = new Hyperlink
            {
                NavigateUri = new Uri(url)
            };

            if (tooltip is not null) hyperlink.ToolTip = tooltip;

            ExplorerHelper.OpenHyperlink(hyperlink);

            hyperlink.Inlines.Add(textBlockHyperlink.Inlines.FirstInline);
            textBlock.Inlines.Add(hyperlink);
            return textBlock;
        }
    }
}