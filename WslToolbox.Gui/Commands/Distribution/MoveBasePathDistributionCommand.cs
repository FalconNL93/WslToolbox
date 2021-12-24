using System;
using System.Diagnostics;
using System.IO;
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
                $"Move:\n{DistributionClass.BasePathLocal}\n\nTo:\n{_selectedBasePath}?",
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

            if (await Summary() != ContentDialogResult.Primary) return;

            _model.StopDistribution.Execute(distribution);
            MoveDistribution(_selectedBasePath);
        }

        private static async Task RemoveDistribution(string file)
        {
            try
            {
                await Task.Run(() => { File.Delete(file); });
            }
            catch (Exception e)
            {
                LogHandler.Log().Error("{Message}", e.Message);
            }
        }

        private async void MoveDistribution(string destination, bool copyMode = true)
        {
            var sourceDirectory = DistributionClass.BasePathLocal;
            var destinationDirectory = destination;
            try
            {
                foreach (var filename in Directory.EnumerateFiles(sourceDirectory))
                {
                    var length = new FileInfo(filename).Length;
                    var humanLength = new BytesToHumanConverter().Convert(length, null, null, null);
                    ShowInfo("Copying",
                        $"Copying file {Path.GetFileName(filename)} ({humanLength})...");
                    await using var sourceStream = File.Open(filename, FileMode.Open);
                    await using var destinationStream =
                        File.Create(destinationDirectory + filename.Substring(filename.LastIndexOf('\\')));
                    await sourceStream.CopyToAsync(destinationStream);
                    await RemoveDistribution(filename);

                    HideInfo();
                }

                ChangeBasePathDistributionCommand.Execute(DistributionClass,
                    destinationDirectory);
            }
            catch (IOException io)
            {
                HideInfo();
                Debug.WriteLine(io.Message);
                LogHandler.Log().Error("{Message}", io.Message);
            }
            catch (Exception e)
            {
                HideInfo();
                LogHandler.Log().Error(e, "MoveBasePathError");
            }
        }
    }
}