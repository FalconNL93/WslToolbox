using System.Windows.Data;
using ModernWpf.Controls;
using WslToolbox.Gui.Collections.TopMenu;
using WslToolbox.Gui.Helpers;
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
                    Flyout = UiElementHelper.MenuFlyoutItems(AddTopMenuCollection.Items(viewModel))
                },
                new DropDownButton
                {
                    Content = "Service...",
                    Flyout = UiElementHelper.MenuFlyoutItems(ServiceTopMenuCollection.Items(viewModel))
                },
                new DropDownButton
                {
                    Content = "More...",
                    Flyout = UiElementHelper.MenuFlyoutItems(MoreTopMenuCollection.Items(viewModel))
                }
            };
        }
    }
}