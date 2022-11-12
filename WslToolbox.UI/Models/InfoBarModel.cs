using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Models;

public class InfoBarModel
{
    public bool IsOpen { get; set; }
    public bool IsClosable { set; get; }
    public bool IsIconVisible { get; set; } = true;
    public InfoBarSeverity Severity { get; set; } = InfoBarSeverity.Informational;
    public string Title { get; set; }
    public string Message { get; set; }
}