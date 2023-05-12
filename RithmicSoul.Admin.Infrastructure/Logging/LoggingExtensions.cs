using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace RithmicSoul.Admin.Infrastructure.Logging;

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