using static System.Reflection.Assembly;

namespace WslToolbox.Gui.Helpers
{
    public class UpdaterVersion
    {
        public int Build;
        public string Version;
    }

    public static class AssemblyHelper
    {
        public static string AssemblyName => GetExecutingAssembly().GetName().Name;
        public static string AssemblyVersionFull => GetExecutingAssembly().GetName().Version?.ToString();

        public static string Version()
        {
            var version =
                $"{GetExecutingAssembly().GetName().Version?.Major}.{GetExecutingAssembly().GetName().Version?.Minor}";

            return version;
        }

        public static int Build()
        {
            var version = GetExecutingAssembly().GetName().Version;

            return version == null
                ? 0
                : version.Build;
        }

        public static UpdaterVersion ConvertUpdaterVersion(string version)
        {
            var split = version.Split(".");
            var build = 0;
            var index = 0;

            foreach (var versionParam in split)
            {
                build = index switch
                {
                    2 => short.Parse(versionParam),
                    _ => build
                };

                index++;
            }

            return new UpdaterVersion
            {
                Version = $"{split[0]}.{split[1]}",
                Build = build
            };
        }
    }
}