using FishNet.Object;
using Microsoft.Extensions.Logging;
using UnityEngine;
using Ward.MultiplayerGame.Logging;

namespace Ward.MultiplayerGame.Other
{
    // Notifies if this object is the first object spawned by the local connection
    public class FirstObjectNotifier : NetworkBehaviour
    {
        public delegate void FirstObjectSpawned(Transform t);
        public static event FirstObjectSpawned OnFirstObjectSpawned;

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (base.IsOwner)
            {
                NetworkObject networkObject = base.LocalConnection.FirstObject;
                if(networkObject == base.NetworkObject) 
                    OnFirstObjectSpawned?.Invoke(transform);
            }
        }
    }
}
