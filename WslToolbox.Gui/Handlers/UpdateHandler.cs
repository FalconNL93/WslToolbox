using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using AutoUpdaterDotNET;
using ModernWpf.Controls;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;

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
        private UpdateInfoEventArgs _updateArgs;
        private bool _showPrompt = true;

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
            _showPrompt = showPrompt;
            if (!IsAvailable()) return;

            var configuration = new UpdateConfiguration();
            SetConfiguration(configuration);

            AutoUpdater.Start(configuration.Url);
        }

        private void OnAutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
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
                    ChangelogUrl = args.ChangelogURL,
                });
        }

        private async void InstallUpdate()
        {
            var fileHttpCode = HttpResponseCode(_updateArgs.DownloadURL);

            if (fileHttpCode != HttpStatusCode.OK)
            {
                await UiHelperDialog.ShowMessageBoxSelectable("Error", "Could not download the update file.",
                        $"URL: {_updateArgs.DownloadURL}\n" +
                        $"Error: {fileHttpCode}\n" +
                        $"New version: {_updateArgs.CurrentVersion}\n" +
                        $"Update Available: {_updateArgs.IsUpdateAvailable}\n" +
                        $"Mandatory: {_updateArgs.Mandatory.Value}")
                    .ShowAsync();
                return;
            }

            try
            {
                if (AutoUpdater.DownloadUpdate(_updateArgs)) Environment.Exit(0);
            }
            catch (Exception e)
            {
                await UiHelperDialog.ShowMessageBoxInfo("Error", "Could not update application.\n\n" +
                                                                 $"{e.Message}").ShowAsync();
            }
        }

        public async void ShowUpdatePrompt()
        {
            var updatePrompt = UiHelperDialog.ShowMessageBoxInfo(
                $"Update available - {_updateArgs.CurrentVersion}",
                $"Version {_updateArgs.CurrentVersion} is available for {AppConfiguration.AppName}.\n\n" +
                "Do you want to install this update now?",
                "Install update", closeButtonText: "Cancel update", dialogOwner: _view);

            var updatePromptResult = await updatePrompt.ShowAsync();

            if (updatePromptResult == ContentDialogResult.Primary)
                InstallUpdate();
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
    }
}