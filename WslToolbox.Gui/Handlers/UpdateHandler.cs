using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using ModernWpf.Controls;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Configurations.Sections;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Handlers
{
    public class UpdateStatusArgs : EventArgs
    {
        public string AppName { get; set; }
        public bool UpdateAvailable { get; set; }
        public Exception UpdateError { get; set; }
        public bool ShowPrompt { get; set; } = true;
        public string CurrentVersion { get; set; }
        public Version InstalledVersion { get; set; }
        public string DownloadUrl { get; set; }
        public string ChangelogUrl { get; set; }
    }

    public class UpdateHandler
    {
        private readonly Window _view;

        public UpdateHandler(Window view)
        {
            _view = view;

            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {

        }

        private static void SetConfiguration(UpdateConfiguration updateConfiguration)
        {

        }

        public static bool IsAvailable()
        {
            return AppConfiguration.EnableUpdater &&
                   AppConfiguration.AppConfigurationUpdateXml != null;
        }

        public void CheckForUpdates(bool showPrompt = true)
        {

        }

        private async void InstallUpdate()
        {

        }

        public async void ShowUpdatePrompt()
        {
            
        }

        private static HttpStatusCode HttpResponseCode(string url)
        {
            HttpWebResponse updateResponse = null;

            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request == null) return HttpStatusCode.ServiceUnavailable;

                request.Method = "HEAD";
                updateResponse = (HttpWebResponse) request.GetResponse();

                return updateResponse.StatusCode;
            }
            catch (WebException we)
            {
                if (we.Status != WebExceptionStatus.ProtocolError) return HttpStatusCode.ServiceUnavailable;

                var response = (HttpWebResponse) we.Response;

                return response?.StatusCode ?? HttpStatusCode.ServiceUnavailable;
            }
            catch
            {
                return HttpStatusCode.ServiceUnavailable;
            }
            finally
            {
                updateResponse?.Close();
            }
        }

        private static async Task<long> DownloadResponse(string url)
        {
            try
            {
                var request = WebRequest.CreateHttp(url);
                request.Method = "HEAD";

                using var response = await request.GetResponseAsync();
                var length = response.ContentLength;

                return length;
            }
            catch
            {
                return 0;
            }
        }
    }
}