using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace RithmicSoul.Admin.Client.Logging;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddConsoleRedirectLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, ConsoleRedirectLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <CustomLoggerConfiguration, ConsoleRedirectLoggerProvider>(builder.Services);

        return builder;
    }
}