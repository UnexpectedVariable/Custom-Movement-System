using Assets.Scripts.Animation.Player;
using Assets.Scripts.Input.Player;
using Assets.Scripts.MovementSystem.Player;
using Assets.Scripts.Physics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#pragma warning disable 0414

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody = null;

        [Header("Movement&Control System")]
        [SerializeField]
        private InputActionAsset _inputAsset = null;
        [SerializeField]
        private List<PlayerInputBinder> _playerInputBinders = null;
        [SerializeField]
        private PlayerMovementController _playerMovementController = null;
        [SerializeField]
        private PlayerRotationController _playerRotationController = null;
        [SerializeField]
        private PlayerAnimationManager _playerAnimationManager = null;
        [SerializeField]
        private PlayerCollisionHandler _playerCollisionHandler = null;
        [SerializeField]
        private PhysicalAgentManager _physicalAgentManager = null;

        public List<PlayerInputBinder> PlayerInputBinders => _playerInputBinders;
        internal PlayerMovementController PlayerMovementController => _playerMovementController;
        public PlayerRotationController PlayerRotationController => _playerRotationController;
        internal PlayerAnimationManager PlayerAnimationManager => _playerAnimationManager;
        internal PlayerCollisionHandler PlayerCollisionHandler => _playerCollisionHandler;
        internal PhysicalAgentManager PhysicalAgentManager => _physicalAgentManager;

        private void Start()
        {
            PlayerRotationController.RotatedAlongX += PlayerMovementController.CalculateMovementVector;

            PlayerMovementController.Attach(PlayerAnimationManager);
        }
    }
}
