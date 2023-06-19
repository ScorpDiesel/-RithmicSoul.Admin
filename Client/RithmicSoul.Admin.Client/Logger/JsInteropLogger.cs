using System.Runtime.InteropServices.JavaScript;
using RithmicSoul.Admin.Application.Interfaces;

namespace RithmicSoul.Admin.Client.Logger;

public class JsInteropLogger : IJsInteropLogger
{
    public void LogDebug(string message)
    {
        ConsoleLoggerInterop.ConsoleDebug(message);
    }

    public void LogInfo(string message)
    {
        ConsoleLoggerInterop.ConsoleInfo(message);
    }

    public void LogWarn(string message)
    {
        ConsoleLoggerInterop.ConsoleWarn(message);
    }

    public void LogError(string message)
    {
        ConsoleLoggerInterop.ConsoleError(message);
    }

    public void LogCritical(string message)
    {
        ConsoleLoggerInterop.DotNetCriticalError(message);
    }
}
internal static partial class ConsoleLoggerInterop
{
    [JSImport("globalThis.console.debug")]
    public static partial void ConsoleDebug(string message);
    [JSImport("globalThis.console.info")]
    public static partial void ConsoleInfo(string message);
    [JSImport("globalThis.console.warn")]
    public static partial void ConsoleWarn(string message);
    [JSImport("globalThis.console.error")]
    public static partial void ConsoleError(string message);
    [JSImport("Blazor._internal.dotNetCriticalError", "blazor-internal")]
    public static partial void DotNetCriticalError(string message);
}