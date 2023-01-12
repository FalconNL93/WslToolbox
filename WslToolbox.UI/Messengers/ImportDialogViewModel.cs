using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.Messengers;

public partial class ImportDialogViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _directory;

    public ContentDialogResult ContentDialogResult;

    [RelayCommand]
    private void BrowseFolder(string initialDirectory)
    {
        var browseFolder = DialogHelper.ShowSelectFolderDialog(initialDirectory);
        if (browseFolder.Result != DialogResult.OK)
        {
            return;
        }

        Directory = browseFolder.Dialog.SelectedPath;
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