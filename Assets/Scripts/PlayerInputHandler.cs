using Assets.Scripts.Events;
using Assets.Scripts.Input.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    internal class PlayerInputHandler : MonoBehaviour, IInputHandler
    {
        [SerializeField]
        EventBinding<MovementEvent> _movementEventBinding;

        private void FixedUpdate()
        {
            
        }

        public void HandleInput()
        {
            
        }

        public void HandleMovementEvent(object o, MovementEvent e)
        {
            //Debug.Log($"{o.GetType().Name} Movement event received! Direction {e.direction}");

        }

        private void OnEnable()
        {
            _movementEventBinding = new EventBinding<MovementEvent>(HandleMovementEvent);
            EventBus<MovementEvent>.Register( _movementEventBinding );

            InputUtil.FindMap("Player Movement").actionTriggered += OnActionTriggered;

            //Debug.Log($"{InputUtil.FindMap(Guid.Parse("ab12da6a-01c1-47db-930f-ccb079da0deb")) == null}");
        }

        private void OnDisable()
        {
            EventBus<MovementEvent>.Deregister( _movementEventBinding );

            //_playerInput.onActionTriggered -= OnActionTriggered;
            //_playerInput.currentActionMap.actionTriggered -= OnActionTriggered;
        }

        private void OnActionTriggered(InputAction.CallbackContext callbackContext)
        {
            Debug.Log($"Action triggered!\n" +
                $"Value type: {callbackContext.valueType}");
        }
    }
}
