using System.Runtime.InteropServices;

namespace WslToolbox.UI.Helpers;

public static class MessageBoxHelper
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern int MessageBox(IntPtr hInstance,
        string lpText,
        string lpCaption,
        uint type
    );

    public static MessageBoxResult Show(string text, string title, uint type = MessageBoxTypes.MbOk)
    {
        var messagebox = MessageBox(0, text, title, type);

        return (MessageBoxResult) messagebox;
    }

    public static MessageBoxResult Show(string text, uint type = MessageBoxTypes.MbOk)
    {
        return Show(text, string.Empty, type);
    }

    public static MessageBoxResult Show(string text)
    {
        return Show(text, string.Empty);
    }

    public static MessageBoxResult ShowError(string text, string title = "")
    {
        return Show(text, title, MessageBoxTypes.MbIconError + MessageBoxTypes.MbYesNo);
    }
}

public static class MessageBoxTypes
{
    public const uint MbOk = 0x0;
    public const uint MbIconAsterisk = 0x00000040;
    public const uint MbYesNo = 0x00000004;
    public const uint MbIconError = 0x00000010;
}

public enum MessageBoxResult
{
    Unknown = 0,
    Ok = 1,
    No = 7,
    Yes = 6
}