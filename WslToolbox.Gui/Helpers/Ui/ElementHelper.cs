using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace WslToolbox.Gui.Helpers.Ui
{
    public static class ElementHelper
    {
        public static CheckBox CheckBox(string name, string content, string bind = null, object source = null,
            string requires = null, Visibility visibility = Visibility.Visible, bool enabled = true,
            bool isChecked = false)
        {
            var checkBox = new CheckBox
            {
                Name = name,
                Content = content,
                Visibility = visibility,
                IsEnabled = enabled,
                IsChecked = isChecked
            };

            if (bind != null)
                checkBox.SetBinding(ToggleButton.IsCheckedProperty, BindHelper.BindingObject(bind, source));

            if (requires != null)
                checkBox.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));

            return checkBox;
        }

        public static ToggleSwitch ToggleSwitch(string name,
            string content = null,
            string bind = null,
            object source = null,
            string requires = null, Visibility visibility = Visibility.Visible,
            string offContent = null,
            bool enabled = true,
            string header = null,
            bool isOn = false,
            string tooltipContent = null, int tabIndex = 0)
        {
            var toggleSwitch = new ToggleSwitch
            {
                Name = name,
                Visibility = visibility,
                IsEnabled = enabled,
                OnContent = content,
                OffContent = offContent ?? content,
                Header = header,
                IsOn = isOn,
                ToolTip = tooltipContent
            };

            if (bind != null)
                toggleSwitch.SetBinding(ModernWpf.Controls.ToggleSwitch.IsOnProperty,
                    BindHelper.BindingObject(bind, source));

            if (requires != null)
                toggleSwitch.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));

            if (tabIndex > 0)
                toggleSwitch.Margin = new Thickness(tabIndex * 10, 0, 0, 0);

            return toggleSwitch;
        }

        public static ComboBox ComboBox(string name, IEnumerable items, string bind, object source,
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

        public static ComboBox ComboBox(string name, Dictionary<int, string> items, string bind, object source,
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

        public static TextBox TextBox(string name, string content = null, string bind = null, object source = null,
            string requires = null, bool isEnabled = false, int width = 0,
            BindingMode bindingMode = BindingMode.Default,
            bool isReadonly = false,
            UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default, string placeholder = null)
        {
            var textBox = new TextBox
            {
                Name = name,
                Text = content ?? "",
                HorizontalAlignment = HorizontalAlignment.Left,
                IsReadOnly = isReadonly
            };

            if (bind != null)
                textBox.SetBinding(System.Windows.Controls.TextBox.TextProperty,
                    BindHelper.BindingObject(bind, source, bindingMode, trigger: updateSourceTrigger));

            if (width > 1)
                textBox.Width = width;

            if (requires != null)
                textBox.SetBinding(UIElement.IsEnabledProperty,
                    BindHelper.BindingObject(requires, source, bindingMode, trigger: updateSourceTrigger));
            else
                textBox.IsEnabled = isEnabled;

            ControlHelper.SetPlaceholderText(textBox, placeholder);

            return textBox;
        }

        public static NumberBox NumberBox(string name, string header, int value, string bind, object source,
            string requires = null, bool enabled = false, int width = 170)
        {
            var numberBox = new NumberBox
            {
                Name = name,
                Header = header,
                Value = value,
                Width = width,
                SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact
            };

            if (requires != null)
                numberBox.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));
            else
                numberBox.IsEnabled = enabled;

            numberBox.SetBinding(Selector.SelectedValueProperty, BindHelper.BindingObject(bind, source));

            return numberBox;
        }

        public static ItemsControl ItemsControl(string bind = null, object source = null)
        {
            var itemsControl = new ItemsControl();

            if (bind != null)
                itemsControl.SetBinding(System.Windows.Controls.ItemsControl.ItemsSourceProperty,
                    BindHelper.BindingObject(bind, source));

            return itemsControl;
        }

        public static ItemsControl ItemsControlGroup(CompositeCollection items, bool itemEnableOverride = false,
            bool enabled = true, object source = null, string requires = null, string header = null, int tabIndex = 0)
        {
            var groupItems = new ItemsControl();

            if (header != null)
                groupItems.Items.Add(new Label
                {
                    Content = header,
                    FontWeight = FontWeights.Bold
                });

            foreach (var item in items)
            {
                if (!item.GetType().IsSubclassOf(typeof(Control)))
                {
                    groupItems.Items.Add(item);
                    continue;
                }

                var controlItem = (Control) item;
                if (requires != null)
                    controlItem.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));

                if (itemEnableOverride) controlItem.IsEnabled = enabled;
                groupItems.Items.Add(item);

                if (tabIndex > 0)
                    controlItem.Margin = new Thickness(tabIndex * 10, 0, 0, 0);
            }

            return groupItems;
        }

        public static Expander Expander(string header, CompositeCollection items, bool expanded = false,
            bool expanderEnabled = true,
            bool controlsEnabled = true)
        {
            return new Expander
            {
                Header = header,
                Content = new ItemsControl {ItemsSource = items, IsEnabled = controlsEnabled},
                IsExpanded = expanded,
                IsEnabled = expanderEnabled
            };
        }

        public static IEnumerable<Control> ItemsListGroup(CompositeCollection items, bool itemEnableOverride = false,
            bool enabled = true, object source = null, string requires = null)
        {
            List<Control> controlItems = new();

            foreach (Control item in items)
            {
                if (requires != null)
                    item.SetBinding(UIElement.IsEnabledProperty, BindHelper.BindingObject(requires, source));

                if (itemEnableOverride) item.IsEnabled = enabled;
                controlItems.Add(item);
            }

            return controlItems;
        }


        public static TextBlock Hyperlink(string url, string name = null, string tooltip = null,
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

        public static Separator Separator(int marginTop = 5, int marginBottom = 10, int marginLeft = 0,
            int marginRight = 0,
            Visibility visibility = Visibility.Hidden)
        {
            return new Separator
            {
                Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom),
                Visibility = visibility
            };
        }

        public static MenuFlyout MenuFlyoutItems(CompositeCollection items)
        {
            var menuFlyout = new MenuFlyout();
            foreach (var menuItem in items)
                menuFlyout.Items.Add(menuItem);

            return menuFlyout;
        }

        public static Button Button(
            string name,
            string content,
            object bindingSource = null,
            string bindingPath = null,
            ICommand command = null,
            object commandParameter = null,
            string requiresBindPath = null,
            string visibilityBindPath = null
        )
        {
            var button = new Button
            {
                Name = name,
                Content = content,
                Command = command,
                CommandParameter = commandParameter
            };

            if (bindingPath != null)
                button.SetBinding(ButtonBase.CommandProperty, BindHelper.BindingObject(bindingPath, bindingSource));

            if (requiresBindPath != null)
                button.SetBinding(UIElement.IsEnabledProperty,
                    BindHelper.BindingObject(requiresBindPath, bindingSource));

            if (visibilityBindPath != null)
                button.SetBinding(UIElement.VisibilityProperty,
                    BindHelper.BindingObject(visibilityBindPath, bindingSource));

            return button;
        }

        public static Button FlyoutButton(Button button, string flyoutContent)
        {
            var flyoutItems = new CompositeCollection
            {
                new TextBlock
                {
                    Text = flyoutContent,
                    TextTrimming = TextTrimming.WordEllipsis,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 400
                }
            };

            FlyoutService.SetFlyout(button, new Flyout {Content = new ItemsControl {ItemsSource = flyoutItems}});

            return button;
        }
    }
}