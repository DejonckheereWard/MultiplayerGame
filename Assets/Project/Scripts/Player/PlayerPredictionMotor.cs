using System;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ward.MultiplayerGame.Player
{
    public class PlayerPredictionMotor : NetworkBehaviour
    {
        private struct MoveData : IReplicateData
        {
            public Vector2 MoveInput;
            public Quaternion LookInput;
            public bool JumpInput;

            private uint _tick;
            public uint GetTick() => _tick;

            public void SetTick(uint value) => _tick = value;

            public void Dispose(){}
        }

        private struct ReconcileData: IReconcileData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;
            
            private uint _tick;
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
            public void Dispose() {}
        }

        [SerializeField] private InputActionReference _movementAction;
        [SerializeField] private InputActionReference _jumpAction;

        [SerializeField] private float _moveSpeed = 5f;
        private Rigidbody _rb;
        private bool _isSubscribed = false;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            _movementAction.action?.Enable();
            _jumpAction.action?.Enable();
            SubscribeToTimeManager();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            _movementAction.action?.Disable();
            _jumpAction.action?.Disable();
            UnsubscribeToTimeManager();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            SubscribeToTimeManager();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            UnsubscribeToTimeManager();
        }

        private void SubscribeToTimeManager()
        {
            if (base.TimeManager == null) return;
            if (_isSubscribed) return;

            base.TimeManager.OnTick += TimeManager_OnTick;
            base.TimeManager.OnPostTick += TimeManager_OnPostTick;
            _isSubscribed = true;
        }

        private void UnsubscribeToTimeManager()
        {
            if (!_isSubscribed) return;

            base.TimeManager.OnTick -= TimeManager_OnTick;
            base.TimeManager.OnPostTick -= TimeManager_OnPostTick;
            _isSubscribed = false;
        }

        private MoveData GatherInput()
        {
            MoveData data = default;
            Vector2 moveInput = _movementAction.action.ReadValue<Vector2>();
            bool jumpInput = _jumpAction.action.triggered;

            if (moveInput.sqrMagnitude < float.Epsilon && !jumpInput) return data;

            return new MoveData
            {
                MoveInput = moveInput,
                JumpInput = jumpInput
            };
        }

        [Replicate]
        private void MovePlayer(MoveData data, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
        {
            Vector3 movement = new Vector3(data.MoveInput.x, 0, data.MoveInput.y) * _moveSpeed;
            _rb.AddForce(_rb.position + movement);
        }

        [Reconcile]
        private void ReconcilePlayer(ReconcileData data, bool asServer, Channel channel = Channel.Unreliable)
        {
            _rb.position = data.Position;
            _rb.rotation = data.Rotation;
            _rb.velocity = data.Velocity;
            _rb.angularVelocity = data.AngularVelocity;
        }

        private void TimeManager_OnTick()
        {
            if (base.IsOwner)
            {
                ReconcilePlayer(default, false);
                MoveData data = GatherInput();
                MovePlayer(data, false);
            }

            if (base.IsServerInitialized)
            {
                MovePlayer(default, true);
            }
        }

        private void TimeManager_OnPostTick()
        {
            ReconcileData data = new ReconcileData
            {
                Position = _rb.position,
                Rotation = _rb.rotation,
                Velocity = _rb.velocity,
                AngularVelocity = _rb.angularVelocity
            };
            ReconcilePlayer(data, true);
        }
    }
}
