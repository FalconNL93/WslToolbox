using System;

namespace WslToolbox.Core.EventArguments
{
    public class FetchEventArguments : EventArgs
    {
        public readonly string Message;

        public FetchEventArguments(string message)
        {
            Message = message;
        }
    }
}