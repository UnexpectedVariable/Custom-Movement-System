using Assets.Scripts.Events;
using Assets.Scripts.Events.Movement.Util;
using Assets.Scripts.Physics;
using Assets.Scripts.Util.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Assets.Scripts.Events.Movement.Util.MovementUtil;

namespace Assets.Scripts.MovementSystem.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    internal class PlayerMovementController : ObservedMonoBehaviour<PlayerMovementController>, IMovementController
    {
        private EventBinding<InputEvent> _movementEventBinding;
        private Rigidbody _rb;

        private Dictionary<MovementType, InputAction> _movementActuationMap;

        private Vector3 _movementVector = Vector3.zero;
        private Vector3 _forceVector = Vector3.zero;

        public Vector3 MovementVector => _movementVector;
        public Vector3 TotalMovementVector
        {
            get
            {
                if (_movementVector == Vector3.zero) return _movementVector;
                CalculateForceVector();
                Debug.Log($"TotalMovementVector calculated to be {_forceVector}");
                return _forceVector;
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

            ModifyActuationMap(type, context.action);
            return true;
        }

        private bool CancelMovement(InputAction.CallbackContext context)
        {
            var type = MovementUtil.GetMovementType(context.action.name);
            if (type == null) return false;

            ModifyActuationMap(type, null);
            return true;
        }

        private void ModifyActuationMap(MovementType? type, InputAction action)
        {
            _movementActuationMap[type.Value] = action;
            CalculateMovementVector();
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
                MovementType.Forward => _rb.transform.forward,
                MovementType.Backward => _rb.transform.forward * -1,
                MovementType.StrafeRight => _rb.transform.right,
                MovementType.StrafeLeft => _rb.transform.right * -1,
                MovementType.Up => _rb.transform.up,
                MovementType.Down => _rb.transform.up * -1,
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

        private void CalculateMovementVector()
        {
            _movementVector = Vector3.zero;
            foreach (var movementPair in _movementActuationMap)
            {
                if (movementPair.Value == null) continue;
                _movementVector += GetMovementDirection(movementPair.Key);
            }

            Notify();
        }

        private void CalculateForceVector()
        {
            _forceVector = Vector3.zero;
            foreach (var actuator in _legActuators)
            {
                _forceVector += actuator.Actuate(_movementVector);
            }
        }

        public void CalculateMovementVector(object sender, EventArgs e)
        {
            //if (sender is not PlayerRotationController) return;
            CalculateMovementVector();
        }

        public override void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Handle(this);
            }
        }
    }
}
