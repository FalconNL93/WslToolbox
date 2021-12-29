using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Validators;

namespace WslToolbox.Gui.Collections.Dialogs
{
    public class RenameDistributionDialogCollection : INotifyPropertyChanged
    {
        private string _distributionName;
        private bool _distributionNameIsValid;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Control> Items(DistributionClass distributionClass)
        {
            var distributionName = new TextBox();
            distributionName.SetBinding(TextBox.TextProperty,
                BindHelper.BindingObject(nameof(DistributionName), this));

            DistributionName = distributionClass.Name;
            distributionName.TextChanged += (_, _) =>
            {
                DistributionNameIsValid = DistributionNameValidator.ValidateRename(
                    distributionName.Text, distributionClass.Name);
            };

            Control[] items =
            {
                new Label {Content = "Name:", Margin = new Thickness(0, 0, 0, 2), FontWeight = FontWeights.Bold},
                new Label
                {
                    Content = "- Only alphanumeric characters are allowed.\n" +
                              "- Name must contain at least 3 characters.",
                    Margin = new Thickness(0, 0, 0, 5)
                },
                distributionName
            };

            return items;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}