using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ward.MultiplayerGame.Player
{
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private InputActionReference _lookAction;
        [SerializeField] private float _sensitivity = 1f;
        [SerializeField] private Transform _cameraSocket;

        private Vector2 _totalLook = Vector2.zero;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            {
                if (base.IsOwner && _lookAction.action != null)
                {
                    _lookAction.action.Enable();
                    _lookAction.action.performed += OnLook;
                }
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            {
                if (base.IsOwner && _lookAction.action != null)
                {
                    _lookAction.action.Disable();
                    _lookAction.action.performed -= OnLook;
                }
            }
        }

        private void OnLook(InputAction.CallbackContext obj)
        {
            if (!base.IsOwner) return;
            Vector2 look = obj.ReadValue<Vector2>();
            _totalLook += look * _sensitivity;
            _totalLook.y = Mathf.Clamp(_totalLook.y, -90, 90);
            _cameraSocket.localRotation = Quaternion.Euler(-_totalLook.y, 0, 0);
            this.transform.rotation = Quaternion.Euler(0, _totalLook.x, 0);
        }
    }
}
