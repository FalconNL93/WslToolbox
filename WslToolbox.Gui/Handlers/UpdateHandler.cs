using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using AutoUpdaterDotNET;
using ModernWpf.Controls;
using WslToolbox.Gui.Configurations;
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
        public Mandatory Mandatory { get; set; }
        public string DownloadUrl { get; set; }
        public string ChangelogUrl { get; set; }
    }

    public class UpdateHandler
    {
        private readonly Window _view;
        private bool _showPrompt = true;
        private UpdateInfoEventArgs _updateArgs;

        public UpdateHandler(Window view)
        {
            _view = view;

            InitializeEventHandlers();
        }

        public static event EventHandler<UpdateStatusArgs> UpdateStatusReceived;

        private void InitializeEventHandlers()
        {
            AutoUpdater.CheckForUpdateEvent += OnAutoUpdaterOnCheckForUpdateEvent;
        }

        private static void SetConfiguration(UpdateConfiguration updateConfiguration)
        {
            AutoUpdater.ShowSkipButton = updateConfiguration.ShowSkipButton;
            AutoUpdater.ShowRemindLaterButton = updateConfiguration.ShowRemindLaterButton;
            AutoUpdater.AppTitle = updateConfiguration.AppTitle;
            AutoUpdater.Mandatory = updateConfiguration.Mandatory;
            AutoUpdater.UpdateMode = updateConfiguration.UpdateMode;
            AutoUpdater.OpenDownloadPage = updateConfiguration.OpenDownloadPage;
            AutoUpdater.ReportErrors = false;
        }

        public static bool IsAvailable()
        {
            return AppConfiguration.EnableUpdater &&
                   AppConfiguration.AppConfigurationUpdateXml != null;
        }

        public void CheckForUpdates(bool showPrompt = true)
        {
            LogHandler.Log().Information("Checking for updates");
            _showPrompt = showPrompt;
            if (!IsAvailable()) return;

            var configuration = new UpdateConfiguration();
            SetConfiguration(configuration);

            AutoUpdater.Start(configuration.Url);
        }

        private void OnAutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            LogHandler.Log().Information("UpdateArgs received from {AppConfigurationUpdateXml}",
                AppConfiguration.AppConfigurationUpdateXml);
            _updateArgs = args;

            UpdateStatusReceived?.Invoke(null,
                new UpdateStatusArgs
                {
                    AppName = AppConfiguration.AppName,
                    UpdateAvailable = args.IsUpdateAvailable,
                    UpdateError = args.Error,
                    ShowPrompt = _showPrompt,
                    CurrentVersion = args.CurrentVersion,
                    InstalledVersion = args.InstalledVersion,
                    Mandatory = args.Mandatory,
                    DownloadUrl = args.DownloadURL,
                    ChangelogUrl = args.ChangelogURL
                });
        }

        private async void InstallUpdate()
        {
            var fileHttpCode = HttpResponseCode(_updateArgs.DownloadURL);

            if (fileHttpCode != HttpStatusCode.OK)
            {
                LogHandler.Log().Error("Error downloading {DownloadUrl} (Http Error: {FileHttpCode})",
                    _updateArgs.DownloadURL,
                    fileHttpCode);
                await DialogHelper.ShowMessageBoxInfo("Error", "Could not download the update file")
                    .ShowAsync();
                return;
            }

            try
            {
                LogHandler.Log().Information("Downloading {DownloadUrl}", _updateArgs.DownloadURL);
                if (AutoUpdater.DownloadUpdate(_updateArgs)) Environment.Exit(0);
            }
            catch (Exception e)
            {
                LogHandler.Log().Error(e, "Error downloading {DownloadUrl}", _updateArgs.DownloadURL);
                await DialogHelper.ShowMessageBoxInfo("Error", "Could not update application.\n\n" +
                                                               $"{e.Message}").ShowAsync();
            }
        }

        public async void ShowUpdatePrompt()
        {
            var responseHeaders = await DownloadResponse(_updateArgs.DownloadURL);
            var readableSize = (responseHeaders / 1024f / 1024f).ToString("F2");
            var splitVersion = AssemblyHelper.ConvertUpdaterVersion(_updateArgs.CurrentVersion);
            var updatePrompt = DialogHelper.ShowMessageBoxInfo(
                $"Update available - {splitVersion.Version} Build {splitVersion.Build}",
                $"Version {splitVersion.Version} Build {splitVersion.Build} is available for {AppConfiguration.AppName}. You have version {AssemblyHelper.Version()} Build {AssemblyHelper.Build()}.\n\n" +
                "After the update file has downloaded, the application will restart to apply the update.\n\n" +
                $"Do you want to install this update now? (Size: {readableSize} MB)",
                "Download and install", "Download manually", "Cancel update",
                dialogOwner: _view);

            var updatePromptResult = await updatePrompt.ShowAsync();

            switch (updatePromptResult)
            {
                case ContentDialogResult.Primary:
                    InstallUpdate();
                    break;
                case ContentDialogResult.Secondary:
                    ExplorerHelper.OpenLocal(_updateArgs.DownloadURL);
                    break;
                case ContentDialogResult.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static HttpStatusCode HttpResponseCode(string url)
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