using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;

namespace Ward.MultiplayerGame.Debug
{
    public class ZLoggerTest : MonoBehaviour
    {
        private readonly ILogger<ZLoggerTest> _logger = LogManager.GetLogger<ZLoggerTest>();

        private void Start()
        {
            _logger.LogDebug("Debug message");
            _logger.ZLogDebug($"Debug message");
        }
    }
}
