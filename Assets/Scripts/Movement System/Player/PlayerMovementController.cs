using Assets.Scripts.Events;
using Assets.Scripts.Events.Movement.Util;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.MovementSystem.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class PlayerMovementController : MonoBehaviour, IMovementController
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
                //return movementVector * _velocityMultiplier;
                //return movementVector * (_movementActuationMap[MovementUtil.MovementType.Up] == null ? _velocityMultiplier : _jumpVelocityMultiplier);
                if (movementVector == Vector3.zero) return movementVector;
                var forceVector = Vector3.zero;
                foreach(var actuator in _legActuators) 
                {
                    forceVector += actuator.Actuate(movementVector);
                }
                Debug.Log($"TotalMovementVector calculated to be {forceVector}");
                return forceVector;
            }
        }

        [Header("Movement parameters")]
        [SerializeField]
        private float _maxVelocity = 1.0f;
        [SerializeField]
        private List<ForceGeneratorActuator> _legActuators = null;

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
            var forceVector = TotalMovementVector;
            _rb.AddForce(forceVector, ForceMode.Force);
            if (forceVector == Vector3.zero) Debug.Log($"{gameObject.name} movement vector is zero");
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
