using System;

namespace WslToolbox.Gui2.Models;

public class DialogControlModel
{
    public string PrimaryButtonName { get; set; } = "OK";
    public string SecondaryButtonName { get; set; } = "Cancel";
    public object? Content { get; set; }
    public Action? PrimaryAction { get; set; }
    public Action? SecondaryAction { get; set; }
}