using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public static async Task<List<DistributionClass>> ReadOnlineDistributions(
            List<DistributionClass> currentDistributions)
        {
            var distros = new List<DistributionClass>();
            OnFetchStarted(new FetchEventArguments(null, Url));

            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(1);
                var response = await httpClient.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                var onlineDistributions =
                    JsonSerializer.Deserialize<OnlineDistributions>(await response.Content.ReadAsStringAsync());

                if (onlineDistributions == null)
                {
                    OnFetchSuccessful(new FetchEventArguments(null, Url));
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
                    IsInstalled = currentDistributions?.Exists(x => x.Name == distribution.Name) ?? false,
                    DefaultUid = 0
                }));
            }
            catch (Exception e)
            {
                OnFetchFailed(new FetchEventArguments(e.Message, Url));
                return distros;
            }

            OnFetchSuccessful(new FetchEventArguments(null, Url));
            return distros;
        }

        private static void OnFetchSuccessful(EventArgs eventArgs)
        {
            FetchSuccessful?.Invoke(null, eventArgs ?? EventArgs.Empty);
        }

        private static void OnFetchStarted(EventArgs eventArgs)
        {
            FetchStarted?.Invoke(null, eventArgs ?? EventArgs.Empty);
        }

        private static void OnFetchFailed(EventArgs eventArgs)
        {
            FetchFailed?.Invoke(null, eventArgs ?? EventArgs.Empty);
        }
    }
}