using System.IO;
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
                Title = "Import",
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

        public static OpenFileDialog OpenFolderDialog()
        {
            return new OpenFileDialog
            {
                Title = "Select distribution base path",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select folder",
                FilterIndex = 1,
                RestoreDirectory = true
            };
        }

        private static string Filter()
        {
            return string.Join("|", FileDialogConfiguration.Filter
                .Select(x => x.Key + "|" + x.Value)
                .ToArray());
        }

        public static string SelectDistributionBasePath()
        {
            OpenFileDialog openLocation = new()
            {
                Title = "Select distribution base path",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select folder",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            var selectedBasePathDialog =
                openLocation.ShowDialog() == null ? null : Path.GetDirectoryName(openLocation.FileName);

            return selectedBasePathDialog;
        }
    }
}