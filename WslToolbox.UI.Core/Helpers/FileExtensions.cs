namespace WslToolbox.UI.Core.Helpers;

public class FileExtensionItem
{
    public string Name { get; set; }
    public string Extension { get; set; }
}

public static class FileExtensions
{
    public static FileExtensionItem Tar = new()
    {
        Name = "Tar",
        Extension = ".tar"
    };
}