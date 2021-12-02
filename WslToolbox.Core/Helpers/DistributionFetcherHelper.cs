using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace WslToolbox.Core.Helpers
{
    public class OnlineDistribution
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Amd64PackageUrl { get; set; }
        public string StoreAppid { get; set; }
    }

    public class OnlineDistributions
    {
        public List<OnlineDistribution> Distributions { get; set; }
    }

    public static class DistributionFetcherHelper
    {
        private const string Url =
            "https://raw.githubusercontent.com/microsoft/WSL/master/distributions/DistributionInfo.json";

        public static List<DistributionClass> ReadOnlineDistributions()
        {
            var distros = new List<DistributionClass>();

            try
            {
                if (WebRequest.Create(Url) is not HttpWebRequest request) return distros;
                var response = (HttpWebResponse) request.GetResponse();
                var encoding = Encoding.ASCII;
                string jsonResponse;
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    jsonResponse = reader.ReadToEnd();
                }

                Debug.WriteLine(jsonResponse);
                var onlineDistributions =
                    JsonSerializer.Deserialize<OnlineDistributions>(jsonResponse);

                if (onlineDistributions == null) return distros;

                distros.AddRange(onlineDistributions.Distributions.Select(distribution => new DistributionClass
                {
                    Name = distribution.Name,
                    State = DistributionClass.StateAvailable,
                    Version = 2,
                    BasePath = null,
                    BasePathLocal = null,
                    IsDefault = false,
                    IsInstalled = false,
                    DefaultUid = 0,
                }));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return distros;
            }

            return distros;
        }
    }
}