using System.Linq;
using Microsoft.Win32;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public static class FileDialogHandler
    {
        public static OpenFileDialog OpenFileDialog()
        {
            return new OpenFileDialog
            {
                Title = "Export",
                Filter = Filter(),
                AddExtension = FileDialogConfiguration.AddExtension,
                DefaultExt = FileDialogConfiguration.DefaultExtension,
                FilterIndex = FileDialogConfiguration.FilterIndex,
                RestoreDirectory = FileDialogConfiguration.RestoreDirectory
            };
        }

        public static SaveFileDialog SaveFileDialog()
        {
            return new SaveFileDialog
            {
                Title = "Export",
                Filter = Filter(),
                AddExtension = FileDialogConfiguration.AddExtension,
                DefaultExt = FileDialogConfiguration.DefaultExtension,
                FilterIndex = FileDialogConfiguration.FilterIndex,
                RestoreDirectory = FileDialogConfiguration.RestoreDirectory,
                OverwritePrompt = FileDialogConfiguration.OverwritePrompt
            };
        }

        private static string Filter()
        {
            return string.Join("|", FileDialogConfiguration.Filter
                .Select(x => x.Key + "|" + x.Value)
                .ToArray());
            ;
        }
    }
}