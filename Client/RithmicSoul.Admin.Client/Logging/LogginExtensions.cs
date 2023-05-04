namespace RithmicSoul.Admin.Client.Logging;

public static class LogginExtensions
{
    public static ILoggingBuilder AddConsoleProvider(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerProvider, ConsoleLogProvider>();
        return builder;
    }
}