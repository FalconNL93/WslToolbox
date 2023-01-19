using WslToolbox.UI.Core.Extensions;

namespace WslToolbox.UI.Core.Args;

public class UserProgressChangedEventArgs : EventArgs
{
    public long TotalBytesDownloaded { get; set; }
    public long TotalBytes { get; set; }

    public string TotalBytesHuman => TotalBytes.ToReadableBytes();
    public string TotalBytesDownloadedHuman => TotalBytesDownloaded.ToReadableBytes();
}