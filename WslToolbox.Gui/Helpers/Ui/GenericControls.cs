using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WslToolbox.Gui.Helpers.Ui
{
    public static class GenericControls
    {
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
    }
}