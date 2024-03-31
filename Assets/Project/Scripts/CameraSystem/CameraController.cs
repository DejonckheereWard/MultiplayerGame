using System;
using Cinemachine;
using FishNet.Object;
using UnityEngine;
using Ward.MultiplayerGame.Other;

namespace Ward.MultiplayerGame.CameraSystem
{
    public class CameraController: MonoBehaviour
    {
        private Transform _attachedSocket = null;
        private bool _isAttached = false;
        
        private void Awake()
        {
            FirstObjectNotifier.OnFirstObjectSpawned += OnFirstObjectSpawned;
        }

        private void OnDestroy()
        {
            FirstObjectNotifier.OnFirstObjectSpawned -= OnFirstObjectSpawned;
        }

        private void OnFirstObjectSpawned(Transform t)
        {
            _attachedSocket = t.gameObject.GetComponentInChildren<CameraSocket>().transform;
            _isAttached = true;
        }

        private void LateUpdate()
        {
            if (_isAttached)
            {
                this.transform.SetPositionAndRotation(_attachedSocket.position, _attachedSocket.rotation);
            }
        }
    }
}
