using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Commands;

namespace WslToolbox.Gui.Collections
{
    public static class GenericMenuCollection
    {
        public static CompositeCollection Items(IEnumerable<CompositeCollection> menuItems)
        {
            var menuItemList = new CompositeCollection();

            foreach (var menuItem in menuItems) menuItemList.Add(menuItem);

            return menuItemList;
        }

        public static CompositeCollection CopyToClipboard(string content = null)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Copy to Clipboard",
                    Command = new CopyToClipboardCommand(),
                    CommandParameter = content,
                    IsEnabled = content != null
                }
            };
        }
    }
}