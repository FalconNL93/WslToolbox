using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls;
using WslToolbox.Gui.Converters;

namespace WslToolbox.Gui.Helpers.Ui
{
    public class WaitHelper : INotifyPropertyChanged
    {
        private string _closeButtonText;
        private string _dialogMessage;
        private string _dialogTitle;
        private Window _owner;
        private string _primaryButtonText;
        private bool _progressRingActive;
        private string _secondaryButtonText;

        public BoolToValueConverter Aa = new()
        {
            TrueValue = "Ja",
            FalseValue = "Nee"
        };

        public string DialogTitle
        {
            get => _dialogTitle;
            set
            {
                if (_dialogTitle == value) return;
                _dialogTitle = value;
                OnPropertyChanged(nameof(DialogTitle));
            }
        }

        public string DialogMessage
        {
            get => _dialogMessage;
            set
            {
                if (_dialogMessage == value) return;
                _dialogMessage = value;
                OnPropertyChanged(nameof(DialogMessage));
            }
        }

        public string PrimaryButtonText
        {
            get => _primaryButtonText;
            set
            {
                if (_primaryButtonText == value) return;
                _primaryButtonText = value;
                OnPropertyChanged(nameof(PrimaryButtonText));
            }
        }

        public string SecondaryButtonText
        {
            get => _secondaryButtonText;
            set
            {
                if (_secondaryButtonText == value) return;
                _secondaryButtonText = value;
                OnPropertyChanged(nameof(SecondaryButtonText));
            }
        }

        public string CloseButtonText
        {
            get => _closeButtonText;
            set
            {
                if (_closeButtonText == value) return;
                _closeButtonText = value;
                OnPropertyChanged(nameof(CloseButtonText));
            }
        }

        public Window Owner
        {
            get => _owner;
            set
            {
                if (_owner == value) return;
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }

        public bool ProgressRingActive
        {
            get => _progressRingActive;
            set
            {
                if (_progressRingActive == value) return;
                _progressRingActive = value;
                OnPropertyChanged(nameof(ProgressRingActive));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ContentDialog WaitDialog()
        {
            var textBlock = new TextBlock
            {
                TextTrimming = TextTrimming.WordEllipsis,
                TextWrapping = TextWrapping.Wrap
            };

            textBlock.SetBinding(TextBlock.TextProperty,
                BindHelper.BindingObject(nameof(DialogMessage), this));

            var progressRing = new ProgressRing
            {
                Margin = new Thickness(0, 15, 0, 0),
                Height = 40,
                Width = 40
            };

            progressRing.SetBinding(ProgressRing.IsActiveProperty,
                BindHelper.BindingObject(nameof(ProgressRingActive), this));

            StackPanel itemStack = new();
            itemStack.Children.Add(textBlock);
            itemStack.Children.Add(progressRing);

            var dialog = new ContentDialog
            {
                PrimaryButtonStyle = ResourceHelper.FindResource("AccentButtonStyle"),
                Content = new ScrollViewer {Content = itemStack},
                Owner = Owner
            };

            dialog.SetBinding(ContentDialog.TitleProperty,
                BindHelper.BindingObject(nameof(DialogTitle), this));

            dialog.SetBinding(ContentDialog.PrimaryButtonTextProperty,
                BindHelper.BindingObject(nameof(PrimaryButtonText), this));

            dialog.SetBinding(ContentDialog.SecondaryButtonTextProperty,
                BindHelper.BindingObject(nameof(SecondaryButtonText), this));

            dialog.SetBinding(ContentDialog.CloseButtonTextProperty,
                BindHelper.BindingObject(nameof(CloseButtonText), this));

            return dialog;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.WriteLine($"{propertyName} changed to {CloseButtonText}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}