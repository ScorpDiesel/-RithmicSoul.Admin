using RithmicSoul.Admin.Client.Logging;

namespace RithmicSoul.Admin.Client.Services;

public class LoggingService
{
    private readonly ILoggerProvider _consoleLogProvider;

    public LoggingService(ILogger consoleLogProvider)
    {
        //_consoleLogProvider = consoleLogProvider;
    }

    //public IReadOnlyList<string> GetLogMessages()
    //{
    //    //return _consoleLogProvider.
    //}
}