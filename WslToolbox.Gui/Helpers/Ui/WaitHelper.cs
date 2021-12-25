using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ModernWpf.Controls;
using ProgressBar = ModernWpf.Controls.ProgressBar;

namespace WslToolbox.Gui.Helpers.Ui
{
    public abstract class WaitHelper : INotifyPropertyChanged
    {
        private string _closeButtonText;
        private string _dialogMessage;
        private string _dialogTitle;
        private Window _owner;
        private string _primaryButtonText;
        private ProgressBar _progressBar;
        private Visibility _progressBarVisibility = Visibility.Collapsed;
        private int _progressValue;
        private string _secondaryButtonText;

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

        public Visibility ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set
            {
                if (_progressBarVisibility == value) return;
                _progressBarVisibility = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
            }
        }

        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                if (_progressValue == value) return;
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected ContentDialog WaitDialog()
        {
            var textBlock = new TextBlock
            {
                TextTrimming = TextTrimming.WordEllipsis,
                TextWrapping = TextWrapping.Wrap
            };

            textBlock.SetBinding(TextBlock.TextProperty,
                BindHelper.BindingObject(nameof(DialogMessage), this));

            _progressBar = new ProgressBar
            {
                Margin = new Thickness(0, 15, 0, 0),
                Width = 130,
                IsIndeterminate = true
            };

            _progressBar.SetBinding(RangeBase.ValueProperty,
                BindHelper.BindingObject(nameof(ProgressValue), this));

            _progressBar.SetBinding(UIElement.VisibilityProperty,
                BindHelper.BindingObject(nameof(ProgressBarVisibility), this));

            StackPanel itemStack = new();
            itemStack.Children.Add(textBlock);
            itemStack.Children.Add(_progressBar);

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

        public void UpdateStatus(string title = null, string content = null, int value = 0)
        {
            DialogTitle = title ?? DialogTitle;
            DialogMessage = content ?? DialogMessage;
            ProgressValue = value;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(ProgressValue))
                _progressBar.IsIndeterminate = ProgressValue < 1;
        }
    }
}