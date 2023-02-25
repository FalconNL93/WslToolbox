namespace WslToolbox.UI.Core.EventArguments;

public class CopyFilesStatusChanged : EventArgs
{
    public readonly FileInfo CurrentFile;

    public CopyFilesStatusChanged(FileInfo currentFile)
    {
        CurrentFile = currentFile;
    }
}