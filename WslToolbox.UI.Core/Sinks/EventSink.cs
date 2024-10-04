using Serilog.Core;
using Serilog.Events;

namespace WslToolbox.UI.Core.Sinks;

public class EventSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
#if DEBUG
        Console.WriteLine($"{logEvent.Timestamp}] {logEvent.MessageTemplate}");
#endif
        NewLogHandler?.Invoke(typeof(EventSink), new LogEventArgs {Log = logEvent});
    }

    public static event EventHandler<LogEventArgs> NewLogHandler;
}

public class LogEventArgs : EventArgs
{
    public LogEvent Log { get; set; }
}