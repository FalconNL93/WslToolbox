namespace WslToolbox.UI.Core.Extensions;

public static class IntExtensions
{
    public static string ToReadableBytes(this long bytesObj)
    {
        string[] suffixNames = {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
        var counter = 0;
        var dValue = decimal.Parse(bytesObj.ToString());
        while (Math.Round(dValue / 1024) >= 1)
        {
            dValue /= 1024;
            counter++;
        }

        return $"{dValue:n1} {suffixNames[counter]}";
    }
}