using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WslToolbox.Gui.Collections
{
    public abstract class Collections
    {
        private readonly object _source;

        protected Collections(object source)
        {
            _source = source;
        }

        private Binding AddBinding(string path, BindingMode mode = BindingMode.Default)
        {
            return new Binding(path)
            {
                Mode = mode,
                Source = _source
            };
        }

        protected CheckBox AddCheckBox(string name, string content, string bind, string requires = null)
        {
            var checkBox = new CheckBox
            {
                Name = name,
                Content = content
            };

            checkBox.SetBinding(ToggleButton.IsCheckedProperty, AddBinding(bind));

            if (requires != null) checkBox.SetBinding(UIElement.IsEnabledProperty, AddBinding(requires));

            return checkBox;
        }
        
        protected ComboBox AddComboBox(string name, IEnumerable items, string bind, string requires = null)
        {
            var comboBox = new ComboBox
            {
                Name = name,
                ItemsSource = items,
            };
            
            if (requires != null) comboBox.SetBinding(UIElement.IsEnabledProperty, AddBinding(requires));
            
            comboBox.SetBinding(Selector.SelectedItemProperty, AddBinding(bind));

            return comboBox;
        }
    }
}