namespace WslToolbox.UI.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
sealed class AppCenterAttribute : Attribute
{
    public string? AppCenterKey { get; }

    public AppCenterAttribute(string? appCenterKey)
    {
        AppCenterKey = appCenterKey;
    }
}