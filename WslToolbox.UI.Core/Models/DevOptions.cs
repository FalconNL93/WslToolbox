namespace WslToolbox.UI.Core.Models;

public enum FakeUpdateResult
{
    Off,
    UpdateAvailable,
    NoUpdate
}

public class DevOptions
{
    public FakeUpdateResult FakeUpdateResult { get; set; } = FakeUpdateResult.Off;
}