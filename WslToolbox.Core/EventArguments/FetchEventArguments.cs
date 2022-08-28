using System;

namespace WslToolbox.Core.EventArguments;

public class FetchEventArguments : EventArgs
{
    public readonly string Message;
    public readonly string Url;

    public FetchEventArguments(string message, string url = null)
    {
        Message = message;
        Url = url;
    }
}