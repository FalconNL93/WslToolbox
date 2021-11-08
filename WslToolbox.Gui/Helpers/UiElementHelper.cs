using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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

            comboBox.SetBinding(Selector.SelectedValueProperty, BindHelper.BindingObject(bind, source));

            return comboBox;
        }

        public static ComboBox AddComboBox(string name, Dictionary<int, string> items, string bind, object source,
            string requires = null)
        {
            var comboBox = new ComboBox
            {
                Name = name,
                ItemsSource = items,
                SelectedValuePath = "Key",
                DisplayMemberPath = "Value"
            };

            if (requires != null)
                comboBox.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));

            comboBox.SetBinding(Selector.SelectedValueProperty, BindHelper.BindingObject(bind, source));

            if (items.Count <= 1) comboBox.IsEnabled = false;

            return comboBox;
        }

        public static TextBox AddTextBox(string name, string content, string bind, object source,
            string requires = null, bool enabled = false)
        {
            var textBox = new TextBox
            {
                Name = name,
                Text = content
            };

            if (requires != null)
                textBox.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));
            else
                textBox.IsEnabled = enabled;

            textBox.SetBinding(Selector.SelectedValueProperty, BindHelper.BindingObject(bind, source));

            return textBox;
        }

        public static ItemsControl AddItemsControl(string bind, object source)
        {
            var itemsControl = new ItemsControl();

            itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, BindHelper.BindingObject(bind, source));

            return itemsControl;
        }

        public static TextBlock AddHyperlink(string url, string name = null, string tooltip = null,
            string bind = null, CompositeCollection contextMenuItems = null)
        {
            var textBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 350,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                Text = name ?? url
            };

            var hyperlink = new Hyperlink
            {
                NavigateUri = new Uri(url),
                ContextMenu = contextMenuItems != null
                    ? new ContextMenu {ItemsSource = contextMenuItems}
                    : null
            };

            if (tooltip is not null) hyperlink.ToolTip = tooltip;

            ExplorerHelper.OpenHyperlink(hyperlink);

            hyperlink.Inlines.Add(textBlock.Inlines.FirstInline);
            textBlock.Inlines.Add(hyperlink);
            return textBlock;
        }

        public static Separator HiddenSeparator()
        {
            return new Separator
            {
                Margin = new Thickness(0, 5, 0, 5),
                Visibility = Visibility.Hidden
            };
        }
    }
}