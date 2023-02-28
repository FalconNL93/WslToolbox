namespace WslToolbox.UI.Core.EventArguments;

public class LogFileChangedEventArgs : EventArgs
{
    public readonly string LogEntry;
    
    public LogFileChangedEventArgs(string logEntry)
    {
        LogEntry = logEntry;
    }
}