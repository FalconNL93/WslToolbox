using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public class DataGridMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new()
            {
                new MenuItem()
                {
                    Header = "Work in Progress",
                    IsEnabled = false
                },
            };
        }
    }
}