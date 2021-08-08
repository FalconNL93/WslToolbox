using System.Collections.Generic;
using System.IO;

namespace WslToolbox
{
    internal class DistributionClass
    {
        public const string StateRunning = "Running";
        public const string StateStopped = "Stopped";
        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public int Version { get; set; }
        public bool isInstalled { get; set; }

        public static List<DistributionClass> FromOutput(string output)
        {
            List<DistributionClass> distros = new();

            using (StringReader reader = new(output))
            {
                var headerLine = reader.ReadLine();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var tabbed = line.Split("\t");
                    DistributionClass distro = new();

                    distro.IsDefault = tabbed[0] == "*" ? distro.IsDefault = true : distro.IsDefault = false;
                    distro.Name = tabbed[1];
                    distro.State = tabbed[2];
                    distro.Version = int.Parse(tabbed[3]);
                    distro.isInstalled = true;

                    distros.Add(distro);
                }
            }

            return distros;
        }

        public static List<DistributionClass> FromAvailableOutput(string output)
        {
            List<DistributionClass> distros = new();

            using (StringReader reader = new(output))
            {
                var headerLine = reader.ReadLine();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "" || line.StartsWith("NAME") || line.StartsWith("Install"))
                    {
                        continue;
                    }

                    var tabbed = line.Split("\t");
                    DistributionClass distro = new();

                    distro.IsDefault = false;
                    distro.Name = tabbed[0];
                    distro.State = "Available";
                    distro.Version = 2;
                    distro.isInstalled = false;

                    distros.Add(distro);
                }
            }

            return distros;
        }
    }
}