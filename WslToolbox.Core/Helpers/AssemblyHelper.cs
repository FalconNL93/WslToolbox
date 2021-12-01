using static System.Reflection.Assembly;

namespace WslToolbox.Core.Helpers
{
    public static class AssemblyHelper
    {
        public static string AssemblyName => GetExecutingAssembly().GetName().Name;
        public static string AssemblyVersionFull => GetExecutingAssembly().GetName().Version?.ToString();

        public static string AssemblyVersionHuman =>
            $"{GetExecutingAssembly().GetName().Version?.Major}.{GetExecutingAssembly().GetName().Version?.Minor}.{GetExecutingAssembly().GetName().Version?.Build}";
    }
}