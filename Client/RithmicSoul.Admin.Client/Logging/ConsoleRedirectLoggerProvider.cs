using Microsoft.JSInterop.WebAssembly;
using System.Collections.Concurrent;

namespace RithmicSoul.Admin.Client.Logging;

public class ConsoleRedirectLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ConsoleRedirectLogger<object>> _loggers;
    private readonly WebAssemblyJSRuntime _jsRuntime;
    private readonly List<string> _logMessages;


    public ConsoleRedirectLoggerProvider(WebAssemblyJSRuntime jsRuntime)
    {
        _loggers = new ConcurrentDictionary<string, ConsoleRedirectLogger<object>>();
        _jsRuntime = jsRuntime;
        _logMessages = new List<string>();
    }

    public ILogger CreateLogger(string name)
    {
        return _loggers.GetOrAdd(name, loggerName => new ConsoleRedirectLogger<object>(name, _jsRuntime));
    }

    public void Dispose()
    {
    }
}