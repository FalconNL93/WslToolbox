using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace WslToolbox.Core.Helpers
{
    public class RegistryHelper
    {
        public static void ChangeKey(DistributionClass distribution, string key, string value)
        {
            if (!OperatingSystem.IsWindows()) return;

            var keyPath = @$"Software\Microsoft\Windows\CurrentVersion\Lxss\{distribution.Guid}";

            try
            {
                var openSubKey =
                    Registry.CurrentUser.OpenSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);

                openSubKey.SetValue(key, value);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static string GetKey(DistributionClass distribution, string key, string defaultValue = "")
        {
            if (!OperatingSystem.IsWindows()) return defaultValue;

            var keyPath = @$"Software\Microsoft\Windows\CurrentVersion\Lxss\{distribution.Guid}";

            try
            {
                var openSubKey =
                    Registry.CurrentUser.OpenSubKey(keyPath, RegistryKeyPermissionCheck.ReadSubTree);

                return openSubKey.GetValue(key, defaultValue).ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return defaultValue;
        }

        public static string DistributionRegistryByName(string name)
        {
            if (!OperatingSystem.IsWindows()) return string.Empty;

            var wslRegistry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Lxss");

            foreach (var wslKey in wslRegistry.GetSubKeyNames())
            {
                if (!wslKey.StartsWith("{") && !wslKey.EndsWith("}")) continue;
                var subKey = wslRegistry.OpenSubKey(wslKey)?.GetValue("DistributionName");
                if ((string) subKey != name) continue;

                return wslKey;
            }

            return string.Empty;
        }
    }
}