using Microsoft.Extensions.Logging;

namespace Ward.MultiplayerGame.Logging
{
    public static class LogManager
    {
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Trace);
        });
    
        public static ILogger<T> GetLogger<T>()
        {
            // standard LoggerFactory caches logger per category so no need to cache in this manager
            return _loggerFactory.CreateLogger<T>();
        }
    }
}
