using System;
using FishNet.Object;
using Microsoft.Extensions.Logging;
using ZLogger;
using UnityEngine;

namespace Ward.MultiplayerGame.Player;

public class Player : NetworkBehaviour
{
    private readonly ILogger<Player> _logger = LogManager.GetLogger<Player>();

    private void Awake()
    {
        _logger.ZLogDebug($"Player awake, owner is: {base.Owner}");
    }
}
