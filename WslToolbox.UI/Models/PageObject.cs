using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Core.Models;

public class PageObject<T>
{
    public Page Page { get; set; }
    public T Obj { get; set; }
}