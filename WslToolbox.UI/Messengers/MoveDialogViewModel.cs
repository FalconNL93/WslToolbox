using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Commands;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Messengers;

public partial class MoveDialogViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _directory;

    [ObservableProperty]
    private Distribution _distribution;
    public OpenUrlCommand OpenUrlCommand { get; } = new();
    
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

public class MoveDialogMessage : AsyncRequestMessage<MoveDialogViewModel>
{
    public MoveDialogMessage(MoveDialogViewModel value)
    {
        ViewViewModel = value;
    }

    public MoveDialogViewModel ViewViewModel { get; set; }
}