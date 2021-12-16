using System.Windows;
using System.Windows.Data;
using ModernWpf.Controls;
using WslToolbox.Gui.Collections.TopMenu;
using WslToolbox.Gui.Configurations;
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
                },
                new DropDownButton
                {
                    Content = "Debug menu",
                    Flyout = ElementHelper.MenuFlyoutItems(DebugTopMenuCollection.Items(viewModel)),
                    Visibility = AppConfiguration.DebugMode ? Visibility.Visible : Visibility.Collapsed
                },
                ElementHelper.Button(
                    "InstallUpdate",
                    "Update available",
                    viewModel,
                    command: viewModel.CheckForUpdates,
                    commandParameter: true,
                    requiresBindPath: nameof(viewModel.UpdateAvailable),
                    visibilityBindPath: nameof(viewModel.UpdateAvailableVisibility)
                )
            };
        }
    }
}