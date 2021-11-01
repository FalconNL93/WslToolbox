using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class OtherCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new Button
                {
                    Content = "Settings",
                    Command = viewModel.ShowSettings
                },
                new Button
                {
                    Content = "Open log",
                    Command = viewModel.OpenLogFile
                },
                new Button
                {
                    Content = "Exit",
                    Command = viewModel.ExitApplication
                }
            };
        }
    }
}