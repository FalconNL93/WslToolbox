using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WslToolbox.Core.Helpers;

namespace WslToolbox.Core
{
    public class DistributionClass
    {
        public const string StateRunning = "Running";
        public const string StateStopped = "Stopped";
        public const string StateAvailable = "Stopped";

        private readonly Dictionary<string, string> _stateCache = new();

        public bool IsDefault { get; set; }
        public bool IsInstalled { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public int Version { get; set; }
        public string Guid { get; set; }
        public string BasePath { get; set; }
        public string BasePathLocal { get; set; }

        public int DefaultUid { get; set; }

        public long Size { get; set; }

        public static async Task<List<DistributionClass>> ListAvailableDistributions(List<DistributionClass> currentDistributions)
        {
            return await DistributionFetcherHelper.ReadOnlineDistributions(currentDistributions);
        }

        public List<DistributionClass> ListDistributions(string output)
        {
            List<DistributionClass> distros = new();
            var distributionGuids = RegistryHelper.ListDistributions();

            foreach (var distributionGuid in distributionGuids)
            {
                DistributionClass distro = new()
                {
                    Guid = distributionGuid
                };

                distro.Name = (string) RegistryHelper.GetKey(distro, "DistributionName");
                distro.State = DistributionState(distro.Name, output);
                distro.Version = int.Parse((string) RegistryHelper.GetKey(distro, "Version"));
                distro.BasePath = (string) RegistryHelper.GetKey(distro, "BasePath");
                distro.BasePathLocal = distro.BasePath.Replace(@"\\?\", "");
                distro.IsDefault = RegistryHelper.DefaultDistributionGuid() == distributionGuid;
                distro.IsInstalled = true;
                distro.DefaultUid = int.Parse((string) RegistryHelper.GetKey(distro, "DefaultUid"));

                var basePathDirectoryInfo = new DirectoryInfo(distro.BasePathLocal);
                var totalSize = basePathDirectoryInfo.EnumerateFiles().Sum(file => file.Length);

                distro.Size = totalSize;

                distros.Add(distro);
            }

            return distros;
        }

        private string DistributionState(string name, string output)
        {
            try
            {
                using StringReader reader = new(output);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var tabbed = line.Split("\t");

                    if (tabbed[1] != name) continue;

                    if (_stateCache.ContainsKey(name))
                        _stateCache[name] = tabbed[2];
                    else
                        _stateCache.Add(name, tabbed[2]);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return _stateCache[name];
        }
    }
}