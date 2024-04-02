using Assets.Scripts.Input.Player;
using Assets.Scripts.MovementSystem.Player;
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

        [Header("Debug")]
        private GameObject _comRepresentation = null;

        [Header("Movement&Control System")]
        [SerializeField]
        private InputActionAsset _inputAsset = null;
        [SerializeField]
        private List<PlayerInputBinder> _playerInputBinders = null;
        [SerializeField]
        private PlayerMovementController _playerMovementController = null;
        public List<PlayerInputBinder> PlayerInputBinders { get => _playerInputBinders; }
        public PlayerMovementController PlayerMovementController { get => _playerMovementController; }

        private void Start()
        {
            InitializeDebugObjects();
        }

        void InitializeDebugObjects()
        {
            _comRepresentation ??= GameObject.Find("CenterOfMassRespresentation");
            _comRepresentation.transform.position = _rigidbody.centerOfMass;
        }
    }
}
