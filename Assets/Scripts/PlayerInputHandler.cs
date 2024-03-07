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
        public InputActionMap Map { get; private set; }

        public PlayerInputHandler(InputActionMap map)
        {
            Map = map;
        }

        public override void Initialize()
        {
            OnInput += RaiseInputEvent;
            InitializeInputMaps();
        }
        public override void Clear()
        {
            ClearInputMaps();
        }

        private void OnPlayerInputTriggered(InputAction.CallbackContext callbackContext)
        {
            HandleEvent(this, new T { InputContext = callbackContext });
            Debug.Log($"Action of type {typeof(T)} triggered!\n" + $"Value type: {callbackContext.valueType}");
        }

        private void InitializeInputMaps()
        {
            //InputUtil.FindMap("Player Movement").actionTriggered += OnPlayerInputTriggered;
            Map.actionTriggered += OnPlayerInputTriggered;
        }

        private void ClearInputMaps()
        {
            //InputUtil.FindMap("Player Movement").actionTriggered -= OnPlayerInputTriggered;
            Map.actionTriggered -= OnPlayerInputTriggered;
        }

        private void RaiseInputEvent(object o, T eventData)
        {
            EventBus<T>.Raise(eventData);
        }
    }
}
