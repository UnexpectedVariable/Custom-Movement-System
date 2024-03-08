using Assets.Scripts.Events;
using Assets.Scripts.Events.Movement.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.MovementSystem.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour, IMovementController, Util.Observer.IObserver<SupportCollidersTracker>
    {
        private EventBinding<InputEvent> _movementEventBinding;
        private Rigidbody _rb;

        private Dictionary<MovementUtil.MovementType, InputAction> _movementActuationMap;

        public Vector3 TotalMovementVector
        {
            get
            {
                var movementVector = Vector3.zero;
                foreach (var movementPair in _movementActuationMap)
                {
                    if (movementPair.Value == null) continue;
                    movementVector += GetMovementDirection(movementPair.Key);
                }
                return movementVector * _velocityMultiplier;
            }
        }

        [Header("Movement parameters")]
        [SerializeField]
        private float _maxVelocity = 1.0f;
        [SerializeField]
        private float _velocityMultiplier = 1.0f;
        [SerializeField]
        private bool _isMovementPossible = default;


        private void Awake()
        {
            _movementEventBinding = new EventBinding<InputEvent>(HandleMovement);
            _rb ??= GetComponent<Rigidbody>();
            InitializeActuationtMap();
        }

        private void InitializeActuationtMap()
        {
            var MovementTypes = Enum.GetValues(typeof(MovementUtil.MovementType));
            int capacity = MovementTypes.Length;
            _movementActuationMap = new Dictionary<MovementUtil.MovementType, InputAction>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                _movementActuationMap.Add((MovementUtil.MovementType)MovementTypes.GetValue(i), null);
            }
        }

        private void Start()
        {
            _rb.maxLinearVelocity = _maxVelocity;
        }

        private void OnEnable()
        {
            EventBus<InputEvent>.Register(_movementEventBinding);
        }

        private void OnDisable()
        {
            EventBus<InputEvent>.Deregister(_movementEventBinding);
        }

        private void FixedUpdate()
        {
            if (_isMovementPossible)
            {
                _rb.AddForce(TotalMovementVector, ForceMode.Force);
            }
            else
            {
                Debug.Log($"{gameObject.name} is unable to move");
            }
        }

        private void HandleMovement(object o, InputEvent e)
        {
            var context = e.InputContext;
            if (context.valueType != typeof(Single)) return;

            Debug.Log("Movement input detected!");
            _ = context.phase switch
            {
                InputActionPhase.Performed => ActuateMovement(context),
                InputActionPhase.Canceled => CancelMovement(context),
                _ => false
            };
        }

        private bool ActuateMovement(InputAction.CallbackContext context)
        {
            var type = MovementUtil.GetMovementType(context.action.name);
            if (type == null) return false;

            _movementActuationMap[type.Value] = context.action;
            return true;
        }

        private bool CancelMovement(InputAction.CallbackContext context)
        {
            var type = MovementUtil.GetMovementType(context.action.name);
            if (type == null) return false;

            _movementActuationMap[type.Value] = null;
            return true;
        }

        private Vector3 GetMovementDirection(InputAction action)
        {
            var movementType = MovementUtil.GetMovementType(action.name);
            return GetMovementDirection(movementType);
        }

        private Vector3 GetMovementDirection(MovementUtil.MovementType? movementType)
        {
            return movementType switch
            {
                MovementUtil.MovementType.Forward => _rb.transform.forward,
                MovementUtil.MovementType.Backward => _rb.transform.forward * -1,
                MovementUtil.MovementType.StrafeRight => _rb.transform.right,
                MovementUtil.MovementType.StrafeLeft => _rb.transform.right * -1,
                MovementUtil.MovementType.Up => _rb.transform.up,
                MovementUtil.MovementType.Down => _rb.transform.up * -1, //is it required?
                _ => Vector3.zero
            };
        }

        public void Handle(SupportCollidersTracker observed)
        {
            var supportCount = observed.SupportColliders.Count;
            if (_isMovementPossible)
            {
                if (supportCount == 0) _isMovementPossible = false;
            }
            else
            {
                if (supportCount > 0) _isMovementPossible = true;
            }
        }

        public void ValidateInputs(InputActionMap actionMap)
        {
            if (_movementActuationMap == null) return;

            for (int i = 0; i < _movementActuationMap.Count; i++)
            {
                var movementPair = _movementActuationMap.ElementAt(i);
                if (movementPair.Value == null) continue;

                foreach (var binding in movementPair.Value.bindings)
                {
                    if (actionMap.bindings.Contains(binding)) continue;
                    _movementActuationMap[movementPair.Key] = null;
                }
            }
        }
    }
}
