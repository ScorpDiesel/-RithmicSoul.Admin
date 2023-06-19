using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using RithmicSoul.Admin.Application.Interfaces;

namespace RithmicSoul.Admin.Infrastructure.Logging;

public class ConsoleRedirectLogger<T> : ILogger<T>, ILogger
{
    private readonly IJsInteropLogger _jsLogger;
    private const string _loglevelPadding = ": ";
    private static readonly string _messagePadding = new(' ', GetLogLevelString(LogLevel.Information).Length + _loglevelPadding.Length);
    private static readonly string _newLineWithMessagePadding = Environment.NewLine + _messagePadding;
    private static readonly StringBuilder _logBuilder = new StringBuilder();

    private readonly string _name;
    private string _logMessage;

    public ConsoleRedirectLogger(IJsInteropLogger jsLogger)
    {
        _jsLogger = jsLogger;
    }

    public ConsoleRedirectLogger(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return NoOpDisposable.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(formatter);

        var message = formatter(state, exception);

        if (!string.IsNullOrEmpty(message) || exception != null)
        {
            WriteMessage(logLevel, _name, eventId.Id, message, exception);
        }
    }

    public string GetLogMessages()
    {
        return _logMessage;
    }

    private void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception? exception)
    {
        lock (_logBuilder)
        {
            try
            {
                CreateDefaultLogMessage(_logBuilder, logLevel, logName, eventId, message, exception);
                var formattedMessage = _logBuilder.ToString();
                _logMessage = formattedMessage;

                switch (logLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                        // Although https://console.spec.whatwg.org/#loglevel-severity claims that
                        // "console.debug" and "console.log" are synonyms, that doesn't match the
                        // behavior of browsers in the real world. Chromium only displays "debug"
                        // messages if you enable "Verbose" in the filter dropdown (which is off
                        // by default). As such "console.debug" is the best choice for messages
                        // with a lower severity level than "Information".
                        _jsLogger.LogDebug(formattedMessage);
                        break;
                    case LogLevel.Information:
                        _jsLogger.LogInfo(formattedMessage);
                        break;
                    case LogLevel.Warning:
                        _jsLogger.LogWarn(formattedMessage);
                        break;
                    case LogLevel.Error:
                        _jsLogger.LogError(formattedMessage);
                        break;
                    case LogLevel.Critical:
                        _jsLogger.LogCritical(formattedMessage);
                        break;
                    default: // invalid enum values
                        Debug.Assert(logLevel != LogLevel.None, "This method is never called with LogLevel.None.");
                        _jsLogger.LogDebug(formattedMessage);
                        //_jsRuntime.InvokeVoid("console.log", formattedMessage);
                        break;
                }
            }
            finally
            {
                _logBuilder.Clear();
            }
        }
    }

    private static void CreateDefaultLogMessage(StringBuilder logBuilder, LogLevel logLevel, string logName, int eventId, string message, Exception? exception)
    {
        logBuilder.Append(GetLogLevelString(logLevel));
        logBuilder.Append(_loglevelPadding);
        logBuilder.Append(logName);
        logBuilder.Append('[');
        logBuilder.Append(eventId);
        logBuilder.Append(']');

        if (!string.IsNullOrEmpty(message))
        {
            // message
            logBuilder.AppendLine();
            logBuilder.Append(_messagePadding);

            var len = logBuilder.Length;
            logBuilder.Append(message);
            logBuilder.Replace(Environment.NewLine, _newLineWithMessagePadding, len, message.Length);
        }

        // Example:
        // System.InvalidOperationException
        //    at Namespace.Class.Function() in File:line X
        if (exception != null)
        {
            // exception message
            logBuilder.AppendLine();
            logBuilder.Append(exception.ToString());
        }
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                return "trce";
            case LogLevel.Debug:
                return "dbug";
            case LogLevel.Information:
                return "info";
            case LogLevel.Warning:
                return "warn";
            case LogLevel.Error:
                return "fail";
            case LogLevel.Critical:
                return "crit";
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel));
        }
    }

    private sealed class NoOpDisposable : IDisposable
    {
        public static NoOpDisposable Instance = new NoOpDisposable();

        public void Dispose() { }
    }
}
