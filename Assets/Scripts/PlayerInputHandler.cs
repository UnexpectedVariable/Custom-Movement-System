using Assets.Scripts.Events;
using Assets.Scripts.Input.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    internal class PlayerInputHandler<T> : InputEventHandler<T> where T : InputEvent, new()
    {
        public override void Initialize()
        {
            InitializeInputMaps();
        }
        public override void Clear()
        {
            ClearInputMaps();
        }

        private void OnPlayerMovementTriggered(InputAction.CallbackContext callbackContext)
        {
            // input = new T { Context = callbackContext };
            EventBus<T>.Raise(new T { Context = callbackContext });
            Debug.Log($"Action triggered!\n" + $"Value type: {callbackContext.valueType}");
        }

        private void InitializeInputMaps()
        {
            InputUtil.FindMap("Player Movement").actionTriggered += OnPlayerMovementTriggered;
        }

        private void ClearInputMaps()
        {
            InputUtil.FindMap("Player Movement").actionTriggered -= OnPlayerMovementTriggered;
        }
    }
}
