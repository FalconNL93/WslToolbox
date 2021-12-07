using static System.Reflection.Assembly;

namespace WslToolbox.Core.Helpers
{
    public static class AssemblyHelper
    {
        public static string AssemblyName => GetExecutingAssembly().GetName().Name;
        public static string AssemblyVersionFull => GetExecutingAssembly().GetName().Version?.ToString();

        public static string Version(bool showZeroBuild = false)
        {
            var version =
                $"{GetExecutingAssembly().GetName().Version?.Major}.{GetExecutingAssembly().GetName().Version?.Minor}";

            if (GetExecutingAssembly().GetName().Version?.Build != 0 || showZeroBuild)
                version = $"{version}.{GetExecutingAssembly().GetName().Version?.Build}";

            return version;
        }
    }
}