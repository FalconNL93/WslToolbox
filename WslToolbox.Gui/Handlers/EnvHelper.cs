using System;
using System.IO;
using System.Linq;

namespace WslToolbox.Gui.Handlers
{
    public static class EnvHelper
    {
        public static string ExecutableEnvironment(string filename)
        {
            if (Environment.GetEnvironmentVariable("PATH") == null)
                return null;

            var paths = new[] {Environment.CurrentDirectory}
                .Concat(Environment.GetEnvironmentVariable("PATH")
                    ?.Split(';') ?? Array.Empty<string>());

            var extensions = new[] {string.Empty}
                .Concat(Environment.GetEnvironmentVariable("PATHEXT")
                    ?.Split(';')
                    .Where(e => e.StartsWith(".")) ?? Array.Empty<string>());

            var combinations = paths.SelectMany(_ => extensions,
                (path, extension) => Path.Combine(path, filename + extension));

            return combinations.FirstOrDefault(File.Exists);
        }
    }
}