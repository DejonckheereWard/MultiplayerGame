using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ward.MultiplayerGame.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private InputActionReference _movementAction;
        [SerializeField] private float _movementSpeed = 5f;

        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (base.IsOwner && _movementAction.action != null)
            {
                _movementAction.action.Enable();
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (base.IsOwner && _movementAction.action != null)
            {
                _movementAction.action.Disable();
            }
        }

        private void Update()
        {
            if (!base.IsOwner) return;
            Vector2 inputDirection = _movementAction.action.ReadValue<Vector2>();
            Vector3 movementDirection = transform.rotation * new Vector3(inputDirection.x, 0, inputDirection.y);
            _rigidBody.velocity = movementDirection * _movementSpeed;
        }
    }
}
