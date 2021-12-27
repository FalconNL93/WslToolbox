using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Gui.Converters;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class MoveBasePathDistribution : GenericDistributionCommand
    {
        private readonly MainViewModel _model;
        private string _selectedBasePath;

        public MoveBasePathDistribution(DistributionClass distributionClass, MainViewModel model) : base(
            distributionClass)
        {
            _model = model;
            IsExecutableDefault = _ => Directory.Exists(distributionClass.BasePathLocal);
            IsExecutable = IsExecutableDefault;
        }

        private static async Task<ContentDialogResult> Warning()
        {
            var moveDistributionWarning = DialogHelper.MessageBox("Warning",
                "All contents inside this base path (even contents which do not belong to this distribution) will be moved to a designated path.",
                "Choose new base path", closeButtonText: "Cancel");

            return await moveDistributionWarning.ShowAsync();
        }

        private async Task<ContentDialogResult> Summary()
        {
            var moveDistributionWarning = DialogHelper.MessageBox("Summary",
                $"Move all contents inside:\n{DistributionClass.BasePathLocal}\n\nTo:\n{_selectedBasePath}?",
                "Move", closeButtonText: "Cancel");

            return await moveDistributionWarning.ShowAsync();
        }

        public override async void Execute(object parameter)
        {
            var distribution = (DistributionClass) parameter;

            if (!_model.Config.Configuration.HideExportWarning)
                if (await Warning() != ContentDialogResult.Primary)
                    return;

            _selectedBasePath = FileDialogHandler.SelectDistributionBasePath();

            if (_selectedBasePath == ""
                || !Directory.Exists(_selectedBasePath)) return;

            if (await Summary() != ContentDialogResult.Primary) return;

            _model.StopDistribution.Execute(distribution);
            MoveDistribution(_selectedBasePath);
        }

        private static async Task RemoveDistributionFiles(List<string> files)
        {
            const int maxTries = 3;
            const int retryWait = 5000;
            await Task.Run(() =>
            {
                foreach (var file in files)
                {
                    var deleteTries = 0;
                    while (File.Exists(file) && deleteTries <= maxTries)
                        try
                        {
                            Debug.WriteLine($"Deleting file {file}");
                            File.Delete(file);
                        }
                        catch (Exception e)
                        {
                            LogHandler.Log().Error("{Message}", e.Message);
                            deleteTries++;
                            Task.Delay(retryWait);
                        }
                }
            });
        }

        private async void MoveDistribution(string destination, bool copyMode = true)
        {
            var sourceDirectory = DistributionClass.BasePathLocal;
            var files = Directory.EnumerateFiles(sourceDirectory).ToList();
            var amountOfFiles = files.Count;
            var currentFile = 1;

            try
            {
                foreach (var filename in files)
                {
                    var length = new FileInfo(filename).Length;
                    var humanLength = new BytesToHumanConverter().Convert(length, null, null, null);
                    ContentDialogHandler.ShowDialog("Copying",
                        $"Copying files ({currentFile}/{amountOfFiles})...\n\n{Path.GetFileName(filename)} ({humanLength})");
                    await using var sourceStream = File.Open(filename, FileMode.Open);
                    await using var destinationStream =
                        File.Create(destination + filename.Substring(filename.LastIndexOf('\\')));
                    await sourceStream.CopyToAsync(destinationStream);
                    currentFile++;
                }

                ChangeBasePathDistributionCommand.Execute(DistributionClass, destination);

                await RemoveDistributionFiles(files);
                ContentDialogHandler.HideDialog();
            }
            catch (IOException io)
            {
                ContentDialogHandler.HideDialog();
                ContentDialogHandler.ShowDialog("Error", $"A read/write error occurred\n\n{io.Message}",
                    closeButtonText: "OK",
                    showCloseButton: true,
                    waitForUser: true
                );

                LogHandler.Log().Error("{Message}", io.Message);
            }
            catch (Exception e)
            {
                ContentDialogHandler.HideDialog();
                ContentDialogHandler.ShowDialog("Error", $"An error occurred\n\n{e.Message}",
                    closeButtonText: "OK",
                    showCloseButton: true,
                    waitForUser: true
                );

                LogHandler.Log().Error(e, "MoveBasePathError");
            }
        }
    }
}