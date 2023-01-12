using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.Messengers;

public partial class ImportDialogViewModel : ObservableRecipient
{
    public string Name { get; set; }
    public string Directory { get; set; }
    public ContentDialogResult ContentDialogResult;

    [RelayCommand]
    private void BrowseFolder()
    {
        try
        {
            var browseFolder = DialogHelper.ShowSelectFolderDialog();
            if (browseFolder.Result != DialogResult.OK)
            {
                return;
            }

            Directory = browseFolder.Dialog.SelectedPath;
        }
        catch (Exception e)
        {

        }
    }
}

public class ImportDialogMessage : AsyncRequestMessage<ImportDialogViewModel>
{
    public ImportDialogMessage(ImportDialogViewModel value)
    {
        ViewViewModel = value;
    }

    public ImportDialogViewModel ViewViewModel { get; set; }
}