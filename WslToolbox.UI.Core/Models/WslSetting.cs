using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public class WslSetting : ObservableObject
{
    private object _value;
    public string Section { get; set; }
    public string Key { get; set; }

    public object Default { get; set; }

    public bool FlagForRemoval { get; set; }

    public ObservableCollection<string>? Options { get; set; }

    public object? Value
    {
        get => _value ?? Default;
        set => SetProperty(ref _value, value);
    }

    public string Description { get; set; }
}