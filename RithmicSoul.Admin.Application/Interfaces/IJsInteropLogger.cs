namespace RithmicSoul.Admin.Application.Interfaces;

public interface IJsInteropLogger
{
    void LogDebug(string message);
    void LogInfo(string message);
    void LogWarn(string message);
    void LogError(string message);
    void LogCritical(string message);
}