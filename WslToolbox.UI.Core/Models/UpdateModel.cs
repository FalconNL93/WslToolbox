namespace WslToolbox.UI.Core.Models;

public class UpdateModel<T>
{
    public T? CurrentModel { get; set; }
    public T? NewModel { get; set; }
}