using Microsoft.Win32;
using System.Diagnostics;

namespace WslToolbox.Gui.Handlers
{
    public class StartOnBootHandler
    {
        private readonly RegistryKey _registryKey;
        private readonly string _appRegistryName;
        private readonly string _appRegistryFileName;
        public bool IsEnabled;
        
        public StartOnBootHandler()
        {
            _appRegistryName = "WSL Toolbox";
            _appRegistryFileName = Process.GetCurrentProcess().MainModule.FileName;
            
            _registryKey = Registry.CurrentUser
                .OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            RegistryExists();
        }
        public void Enable()
        {
            _registryKey.SetValue(_appRegistryName, _appRegistryFileName);
            RegistryExists();
        }

        public void Disable()
        {
            _registryKey.DeleteValue(_appRegistryName,false);
            RegistryExists();
        }

        private void RegistryExists()
        {
            IsEnabled = _registryKey.GetValue(_appRegistryName) != null;
        }
    }
}