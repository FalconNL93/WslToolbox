using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.Validators;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Dialogs
{
    public sealed class ImportDistributionDialogCollection : INotifyPropertyChanged
    {
        private bool _distributionNameIsValid;
        public string DistributionName;
        public string SelectedBasePath;

        public string SelectedFilePath;

        public bool DistributionNameIsValid
        {
            get => _distributionNameIsValid;
            set
            {
                if (value == _distributionNameIsValid) return;
                _distributionNameIsValid = value;
                OnPropertyChanged(nameof(DistributionNameIsValid));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Control> Items(MainViewModel viewModel)
        {
            var distributionFile = new TextBox
                {IsEnabled = false, IsReadOnly = true, Margin = new Thickness(0, 0, 0, 2)};
            var distributionFileBrowse = new Button {Content = "Browse...", Margin = new Thickness(0, 0, 0, 10)};

            var distributionBasePath = ElementHelper.AddTextBox(nameof(DefaultConfiguration.UserBasePath),
                null, "Configuration.UserBasePath", viewModel.Config, width: 400, bindingMode: BindingMode.OneWay);

            var distributionBasePathBrowse = new Button {Content = "Browse...", Margin = new Thickness(0, 0, 0, 10)};
            var distributionName = new TextBox {Margin = new Thickness(0, 0, 0, 10)};

            distributionFileBrowse.Click += (_, _) => { distributionFile.Text = SelectDistributionFile(); };
            distributionBasePathBrowse.Click += (_, _) => { distributionBasePath.Text = SelectDistributionBasePath(); };
            distributionName.TextChanged += (_, _) =>
            {
                DistributionNameIsValid =
                    ValidateImportValues(distributionFile.Text, distributionBasePath.Text, distributionName.Text);

                SelectedFilePath = DistributionNameIsValid ? distributionFile.Text : null;
                SelectedBasePath = DistributionNameIsValid ? distributionBasePath.Text : null;
                SelectedBasePath = DistributionNameIsValid ? distributionBasePath.Text : null;
                DistributionName = DistributionNameIsValid ? distributionName.Text : null;
            };

            Control[] items =
            {
                new Label {Content = "Name:", Margin = new Thickness(0, 0, 0, 2), FontWeight = FontWeights.Bold},
                new Label
                {
                    Content = "- Only alphanumeric characters are allowed.\n" +
                              "- Name must contain atleast 3 characters.",
                    Margin = new Thickness(0, 0, 0, 5)
                },
                distributionName,
                new Label {Content = "Filename:", Margin = new Thickness(0, 0, 0, 2), FontWeight = FontWeights.Bold},
                new Label
                {
                    Content = "Select the file which needs to be imported\n"
                },
                distributionFile,
                ElementHelper.Separator(),
                distributionFileBrowse,

                new Label {Content = "Base path:", Margin = new Thickness(0, 0, 0, 2), FontWeight = FontWeights.Bold},
                distributionBasePath,
                ElementHelper.Separator(),
                distributionBasePathBrowse
            };

            return items;
        }

        private static string SelectDistributionFile()
        {
            var distributionFilePathDialog = FileDialogHandler.OpenFileDialog();

            return distributionFilePathDialog.ShowDialog() == null
                ? null
                : distributionFilePathDialog.FileName;
        }

        private static string SelectDistributionBasePath()
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

            return openLocation.ShowDialog() == null ? null : Path.GetDirectoryName(openLocation.FileName);
        }

        private static bool ValidateImportValues(string distributionFile, string distributionBasePath,
            string distributionName)
        {
            return
                distributionFile.Length >= 1 && distributionBasePath.Length >= 1 &&
                File.Exists(distributionFile) && Directory.Exists(distributionBasePath) &&
                DistributionNameValidator.ValidateName(distributionName);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}