using System.Collections.Generic;
using System.IO;

namespace WslToolbox
{
    internal class DistributionClass
    {
        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public int Version { get; set; }

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

                    distros.Add(distro);
                }
            }

            return distros;
        }
    }
}