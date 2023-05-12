using Microsoft.Extensions.Logging;

namespace RithmicSoul.Admin.Infrastructure.Logging;

public class CustomLoggerConfiguration
{
    public int EventId { get; set; }

    public Dictionary<LogLevel, LogFormat> LogLevels { get; set; } =
        new()
        {
            [LogLevel.Information] = LogFormat.Short,
            [LogLevel.Warning] = LogFormat.Short,
            [LogLevel.Error] = LogFormat.Long
        };

    public enum LogFormat
    {
        Short,
        Long
    }
}