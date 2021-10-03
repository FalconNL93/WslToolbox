using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Helpers
{
    public static class UiElementHelper
    {
        private static Binding AddBinding(string path, object source, BindingMode mode = BindingMode.Default)
        {
            return new Binding(path)
            {
                Mode = mode,
                Source = source
            };
        }

        public static CheckBox AddCheckBox(string name, string content, string bind, object source,
            string requires = null)
        {
            var checkBox = new CheckBox
            {
                Name = name,
                Content = content
            };

            checkBox.SetBinding(ToggleButton.IsCheckedProperty, AddBinding(bind, source));

            if (requires != null) checkBox.SetBinding(UIElement.IsEnabledProperty, AddBinding(requires, source));

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

            if (requires != null) comboBox.SetBinding(UIElement.IsEnabledProperty, AddBinding(requires, source));

            comboBox.SetBinding(Selector.SelectedItemProperty, AddBinding(bind, source));

            return comboBox;
        }

        public static TextBlock AddHyperlink(string url, string name = null, string tooltip = null,
            string bind = null)
        {
            var textBlock = new TextBlock
            {
                Padding = new Thickness(5, 0, 0, 10),
                MaxWidth = 400
            };

            var textBlockHyperlink = new TextBlock
            {
                MaxWidth = 400,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                Text = name ?? url
            };

            var hyperlink = new Hyperlink(textBlockHyperlink.Inlines.LastInline)
            {
                NavigateUri = new Uri(url)
            };

            if (tooltip is not null) hyperlink.ToolTip = tooltip;

            RegisterHyperlinkRequestNavigate(hyperlink);

            textBlock.Inlines.Add(hyperlink);
            return textBlock;
        }

        private static void RegisterHyperlinkRequestNavigate(Hyperlink hyperlink)
        {
            hyperlink.RequestNavigate += (sender, e) =>
            {
                try
                {
                    _ = Process.Start(new ProcessStartInfo("explorer")
                    {
                        Arguments = e.Uri.ToString()
                    });
                }
                catch (Exception ex)
                {
                    LogHandler.Log().Error(ex, ex.Message);
                }
            };
        }
    }
}