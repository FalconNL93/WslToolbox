using System.Windows.Data;
using ModernWpf.Controls;
using WslToolbox.Gui.Collections.TopMenu;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class TopMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new DropDownButton
                {
                    Content = "Add...",
                    Flyout = ElementHelper.MenuFlyoutItems(AddTopMenuCollection.Items(viewModel))
                },
                new DropDownButton
                {
                    Content = "Service...",
                    Flyout = ElementHelper.MenuFlyoutItems(ServiceTopMenuCollection.Items(viewModel))
                },
                new DropDownButton
                {
                    Content = "More...",
                    Flyout = ElementHelper.MenuFlyoutItems(MoreTopMenuCollection.Items(viewModel))
                }
            };
        }
    }
}