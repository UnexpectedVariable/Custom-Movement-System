using Assets.Scripts.Events;
using Assets.Scripts.Events.Movement.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerMovementController : MonoBehaviour, IMovementController, IObserver<SupportCollidersTracker>
    {
        private EventBinding<InputEvent> _movementEventBinding;
        private Rigidbody _rb;
        private Vector3 _movementVector;

        public Vector3 MovementVector
        {
            get => _movementVector;
            private set => _movementVector = value;
        }

        public Vector3 TotalMovementVector
        {
            get => _movementVector * _velocityMultiplier;
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
            if(_isMovementPossible)
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
            var context = e.Context;
            if (context.valueType != typeof(Single)) return;
            //add logic that recognizes if the action can be performed at all
            //aka you can't strafe in or jump off of an air


            Debug.Log("Movement input detected!");
            var direction = GetMovementDirection(context.action);
            _ = context.phase switch
            {
                InputActionPhase.Performed => MovementVector += direction,
                InputActionPhase.Canceled => MovementVector -= direction,
                _ => MovementVector += Vector3.zero
            };
        }

        private Vector3 GetMovementDirection(InputAction action)
        {
            var movementType = MovementUtil.GetMovementType(action.name);
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
            if(_isMovementPossible)
            {
                if(supportCount == 0) _isMovementPossible = false;
            }
            else
            {
                if(supportCount > 0) _isMovementPossible = true;
            }
        }
    }
}
