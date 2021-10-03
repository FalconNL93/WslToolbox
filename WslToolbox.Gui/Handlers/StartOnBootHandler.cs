using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using WslToolbox.Gui.Annotations;

namespace WslToolbox.Gui.Handlers
{
    public class StartOnBootHandler : INotifyPropertyChanged
    {
        private readonly string _appRegistryFileName;
        private readonly string _appRegistryName;
        private readonly RegistryKey _registryKey;
        private bool _isEnabled;

        public StartOnBootHandler()
        {
            _appRegistryName = Assembly.GetExecutingAssembly().GetName().Name;
            _appRegistryFileName = Process.GetCurrentProcess().MainModule.FileName;

            _registryKey = Registry.CurrentUser
                .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            RegistryExists();
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                StartOnBoot(value);
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void StartOnBoot(bool enable)
        {
            if (enable)
                _registryKey.SetValue(_appRegistryName, _appRegistryFileName);
            else
                _registryKey.DeleteValue(_appRegistryName, false);
        }

        private void RegistryExists()
        {
            IsEnabled = _registryKey.GetValue(_appRegistryName) != null;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}