using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Core;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.Validators;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Dialogs
{
    public sealed class ImportDistributionDialogCollection : INotifyPropertyChanged
    {
        private string _distributionName;

        private bool _distributionNameIsValid;
        private bool _runAfterImport;

        private string _selectedBasePath;

        private string _selectedFilePath;

        public string DistributionName
        {
            get => _distributionName;
            set
            {
                if (value == _distributionName) return;
                _distributionName = value;
                OnPropertyChanged(nameof(DistributionName));
            }
        }

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

        public string SelectedBasePath
        {
            get => _selectedBasePath;
            set
            {
                if (value == _selectedBasePath) return;
                _selectedBasePath = value;
                OnPropertyChanged(nameof(SelectedBasePath));
            }
        }

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                if (value == _selectedFilePath) return;
                _selectedFilePath = value;
                OnPropertyChanged(nameof(SelectedFilePath));
            }
        }

        public bool RunAfterImport
        {
            get => _runAfterImport;
            set
            {
                if (value == _runAfterImport) return;
                _runAfterImport = value;
                OnPropertyChanged(nameof(RunAfterImport));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Control> Items(MainViewModel viewModel)
        {
            var createFolder = viewModel.Config.Configuration.GeneralConfiguration.ImportCreateFolder;
            var userBasePath = viewModel.Config.Configuration.UserBasePath;
            var distributionFileBrowse = new Button {Content = "Browse...", TabIndex = 0};
            var distributionBasePathBrowse = new Button {Content = "Browse..."};

            SelectedBasePath = Directory.Exists(userBasePath) ? userBasePath : null;
            distributionFileBrowse.Click += (_, _) => { SelectDistributionFile(); };
            distributionBasePathBrowse.Click += (_, _) =>
            {
                SelectDistributionBasePath(createFolder, DistributionName);
            };

            Control[] items =
            {
                new Label {Content = "Filename:", Margin = new Thickness(0, 0, 0, 2), FontWeight = FontWeights.Bold},
                ElementHelper.TextBox(nameof(SelectedFilePath),
                    null, "SelectedFilePath", this, width: 400,
                    bindingMode: BindingMode.TwoWay, updateSourceTrigger: UpdateSourceTrigger.PropertyChanged,
                    placeholder: "Select an exported distribution file."),
                ElementHelper.Separator(0),
                distributionFileBrowse,

                ElementHelper.Separator(),
                new Label {Content = "Base path:", Margin = new Thickness(0, 0, 0, 2), FontWeight = FontWeights.Bold},
                ElementHelper.TextBox(nameof(SelectedBasePath),
                    bind: "SelectedBasePath", source: this, width: 400, bindingMode: BindingMode.TwoWay,
                    updateSourceTrigger: UpdateSourceTrigger.PropertyChanged,
                    placeholder: "Select an installation directory"),
                ElementHelper.Separator(0),
                distributionBasePathBrowse,
                ElementHelper.Separator(),
                new Label {Content = "Name:", Margin = new Thickness(0, 0, 0, 2), FontWeight = FontWeights.Bold},
                new Label
                {
                    Content = "- Only alphanumeric characters are allowed.\n" +
                              "- Name must contain at least 3 characters.",
                    Margin = new Thickness(0, 0, 0, 10)
                },
                ElementHelper.TextBox(nameof(DistributionName), bind: "DistributionName", width: 400,
                    source: this,
                    isReadonly: false, isEnabled: true, updateSourceTrigger: UpdateSourceTrigger.PropertyChanged,
                    placeholder: "Name your distribution"),
                ElementHelper.Separator()
            };

            return items;
        }

        private void SelectDistributionFile()
        {
            var distributionFilePathDialog = FileDialogHandler.OpenFileDialog();

            SelectedFilePath = distributionFilePathDialog.ShowDialog() == null
                ? null
                : distributionFilePathDialog.FileName;
        }

        private void SelectDistributionBasePath(bool createFolder, string name = "distribution")
        {
            var selectedBasePathDialog = FileDialogHandler.SelectDistributionBasePath();

            SelectedBasePath = createFolder
                ? $"{selectedBasePathDialog}\\{name}"
                : selectedBasePathDialog;
        }

        private bool ValidateImportValues()
        {
            return DistributionName != null
                   && DistributionNameValidator.ValidateName(DistributionName)
                   && SelectedBasePath is {Length: > 1}
                   && SelectedFilePath is {Length: > 1}
                   && File.Exists(SelectedFilePath);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            try
            {
                DistributionNameIsValid = ValidateImportValues();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            if (propertyName == nameof(SelectedFilePath) && !string.IsNullOrEmpty(SelectedFilePath))
                OnSelectedFilePathChanged();
        }

        private void OnSelectedFilePathChanged()
        {
            var fileName = Path.GetFileNameWithoutExtension(SelectedFilePath);
            var distroNameExists = ToolboxClass.DistributionByName(fileName) != null;
            var distroNum = 0;

            while (distroNameExists)
            {
                distroNum++;
                distroNameExists = ToolboxClass.DistributionByName($"{fileName}{distroNum}") != null;
            }

            DistributionName = distroNum > 0
                ? Path.GetFileNameWithoutExtension($"{fileName}{distroNum}")
                : Path.GetFileNameWithoutExtension($"{fileName}");
        }
    }
}