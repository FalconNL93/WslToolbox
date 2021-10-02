using System;
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

        private Binding AddBinding(string path)
        {
            var binding = new Binding(path)
            {
                Mode = BindingMode.Default,
                Source = _source
            };

            return binding;
        }

        protected CheckBox AddCheckBox(string name, string content, string bind)
        {
            var newCheckBox = new CheckBox
            {
                Name = name,
                Content = content
            };

            newCheckBox.SetBinding(ToggleButton.IsCheckedProperty, AddBinding(bind));

            return newCheckBox;
        }
    }
}