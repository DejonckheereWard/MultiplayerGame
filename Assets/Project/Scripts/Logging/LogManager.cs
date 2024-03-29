using Microsoft.Extensions.Logging;
using ZLogger;
using ZLogger.Unity;

public static class LogManager
{
    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(logging =>
    {
        logging.AddZLoggerConsole();
        logging.SetMinimumLevel(LogLevel.Trace);
        logging.AddZLoggerUnityDebug(); // log to UnityDebug
    });
    
    public static ILogger<T> GetLogger<T>()
    {
        // standard LoggerFactory caches logger per category so no need to cache in this manager
        return _loggerFactory.CreateLogger<T>();
    }
}
