using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace RithmicSoul.Admin.Infrastructure.Logging;

public class ConsoleRedirectLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ConsoleRedirectLogger<object>> _loggers;
    private readonly List<string> _logMessages;


    public ConsoleRedirectLoggerProvider()
    {
        _loggers = new ConcurrentDictionary<string, ConsoleRedirectLogger<object>>();
        _logMessages = new List<string>();
    }

    public ILogger CreateLogger(string name)
    {
        return _loggers.GetOrAdd(name, loggerName => new ConsoleRedirectLogger<object>(name));
    }

    public void Dispose()
    {
    }
}