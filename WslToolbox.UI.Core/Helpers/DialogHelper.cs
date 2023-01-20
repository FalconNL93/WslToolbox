namespace WslToolbox.UI.Core.Helpers;

public class DialogResult<T>
{
    public T Dialog;
    public DialogResult Result;
}

public class DialogHelper
{
    public static DialogResult<FolderBrowserDialog> ShowSelectFolderDialog(string initialDirectory)
    {
        using var dialog = new FolderBrowserDialog
        {
            InitialDirectory = initialDirectory
        };

        return new DialogResult<FolderBrowserDialog>
        {
            Result = dialog.ShowDialog(),
            Dialog = dialog
        };
    }

    public static DialogResult<SaveFileDialog> ShowSaveFileDialog()
    {
        return ShowSaveFileDialog(new SaveFileDialog());
    }

    public static DialogResult<SaveFileDialog> ShowSaveFileDialog(SaveFileDialog dialog)
    {
        return new DialogResult<SaveFileDialog>
        {
            Result = dialog.ShowDialog(),
            Dialog = dialog
        };
    }

    public static DialogResult<OpenFileDialog> ShowOpenFileDialog()
    {
        return ShowOpenFileDialog(new OpenFileDialog());
    }

    public static DialogResult<OpenFileDialog> ShowOpenFileDialog(OpenFileDialog dialog)
    {
        return new DialogResult<OpenFileDialog>
        {
            Result = dialog.ShowDialog(),
            Dialog = dialog
        };
    }

    public static string ExtensionFilter(Dictionary<string, string> extensions)
    {
        return string.Join("|", extensions.Select(kv => $"{kv.Key}|*{kv.Value}").ToArray());
    }
}