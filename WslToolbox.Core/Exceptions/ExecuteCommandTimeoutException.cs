using System;

namespace WslToolbox.Core.Exceptions;

internal class ExecuteCommandTimeoutException : Exception
{
    public ExecuteCommandTimeoutException()
    {
    }

    public ExecuteCommandTimeoutException(string message)
        : base(message)
    {
    }

    public ExecuteCommandTimeoutException(string message, Exception inner)
        : base(message, inner)
    {
    }
}