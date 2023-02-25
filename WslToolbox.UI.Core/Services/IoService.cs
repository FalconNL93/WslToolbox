using WslToolbox.Core.Legacy;
using WslToolbox.UI.Core.EventArguments;

namespace WslToolbox.UI.Core.Services;

public abstract class IoService
{
    public static event EventHandler CopyDirectoryStarted;
    public static event EventHandler<CopyFilesStatusChanged> CopyDirectoryStatusChanged;
    public static event EventHandler CopyDirectoryFinished;

    public static async Task CopyDirectory(string source, string destination, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(source))
        {
            throw new Exception("Invalid directory");
        }

        var directory = new DirectoryInfo(source);
        var files = directory.GetFiles().ToList();

        if (!files.Any())
        {
            throw new Exception("No files in source directory");
        }

        CopyDirectoryStarted?.Invoke(source, EventArgs.Empty);
        foreach (var file in files)
        {
            CopyDirectoryStatusChanged?.Invoke(source, new CopyFilesStatusChanged(file));
            await using var sourceStream = File.Open(file.FullName, FileMode.Open, FileAccess.Read);
            await using var destinationStream = File.Create($"{destination}\\{file.Name}");
            await sourceStream.CopyToAsync(destinationStream, cancellationToken);
            sourceStream.Close();
            destinationStream.Close();
        }

        ToolboxClass.OnRefreshRequired();
        CopyDirectoryFinished?.Invoke(source, EventArgs.Empty);
    }
}