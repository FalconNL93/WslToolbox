using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Contracts.Views;

public abstract class ModalPage : Page
{
    public object SelectedItem { get; set; } = new();

    public T GetSelectedItem<T>() where T : class
    {
        if (SelectedItem.GetType() != typeof(T))
        {
            return Activator.CreateInstance<T>();
        }

        return (T) SelectedItem;
    }
}