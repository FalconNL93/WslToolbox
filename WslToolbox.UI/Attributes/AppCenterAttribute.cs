namespace WslToolbox.UI.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class AppCenterAttribute : Attribute
{
    public AppCenterAttribute(string? appCenterKey)
    {
        AppCenterKey = appCenterKey;
    }

    public string? AppCenterKey { get; }
}