using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WslToolbox.Core.EventArguments;

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

        public static event EventHandler FetchStarted;
        public static event EventHandler FetchFailed;
        public static event EventHandler FetchSuccessful;

        public static async Task<List<DistributionClass>> ReadOnlineDistributions()
        {
            var distros = new List<DistributionClass>();
            OnFetchStarted();

            try
            {
                if (WebRequest.Create(Url) is not HttpWebRequest request) return distros;
                var response = await request.GetResponseAsync();
                var encoding = Encoding.ASCII;
                var jsonResponse = new StreamReader(response.GetResponseStream(), encoding);
                var onlineDistributions =
                    JsonSerializer.Deserialize<OnlineDistributions>(await jsonResponse.ReadToEndAsync());

                if (onlineDistributions == null)
                {
                    OnFetchSuccessful();
                    return distros;
                }

                distros.AddRange(onlineDistributions.Distributions.Select(distribution => new DistributionClass
                {
                    Name = distribution.Name,
                    State = DistributionClass.StateAvailable,
                    Version = 2,
                    BasePath = null,
                    BasePathLocal = null,
                    IsDefault = false,
                    IsInstalled = false,
                    DefaultUid = 0
                }));
            }
            catch (Exception e)
            {
                OnFetchFailed(new FetchEventArguments(e.Message));
                return distros;
            }

            OnFetchSuccessful();
            return distros;
        }

        private static void OnFetchSuccessful()
        {
            FetchSuccessful?.Invoke(null, EventArgs.Empty);
        }

        private static void OnFetchStarted()
        {
            FetchStarted?.Invoke(null, EventArgs.Empty);
        }

        private static void OnFetchFailed(FetchEventArguments eventArgs = null)
        {
            FetchFailed?.Invoke(null, eventArgs ?? EventArgs.Empty);
        }
    }
}