using Microsoft.JSInterop;

namespace RithmicSoul.Admin.Client.Logging;

public class ConsoleLogProvider : ILoggerProvider
{
    private readonly List<string> _logMessages;

    public ConsoleLogProvider()
    {
        _logMessages = new List<string>();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new ConsoleLogger(_logMessages);
    }

    public void Dispose()
    {
        // Perform any cleanup if necessary
    }

    public IReadOnlyList<string> GetLogMessages()
    {
        return _logMessages;
    }
}

public class ConsoleLogger : ILogger
{
    private readonly List<string> _logMessages;

    public ConsoleLogger(List<string> logMessages)
    {
        _logMessages = logMessages ?? throw new ArgumentNullException(nameof(logMessages));
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return NoopDisposable.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = $"{logLevel}: {formatter(state, exception)}";
        _logMessages.Add(message);
    }

    public IReadOnlyList<string> GetLogMessages()
    {
        return _logMessages;
    }

    private class NoopDisposable : IDisposable
    {
        public static readonly NoopDisposable Instance = new NoopDisposable();

        public void Dispose()
        {
            // Do nothing
        }
    }

}