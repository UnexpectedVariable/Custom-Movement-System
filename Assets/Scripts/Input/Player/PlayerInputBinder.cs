using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input.Player
{
    [CreateAssetMenu(fileName = "Player Input Binder", menuName = "Input/Binders/Player Input Binder", order = 0)]
    public class PlayerInputBinder : ScriptableObject
    {
        private InputEventHandler<InputEvent> _inputHandler = null;
        private InputActionMap _actionMap = null;

        private bool _remapPending = false;
        public bool AutomaticValidation = true;
        public InputActionMap ActionMap
        {
            set
            {
                _actionMap = value;
                _remapPending = true;

                if (AutomaticValidation) ValidateActionMap();
            }
        }

        public PlayerInputBinder(InputActionMap actionMap)
        {
            _actionMap = actionMap;

            InitializeHandler();
        }

        public void ValidateActionMap()
        {
            if (!_remapPending) return;
            _remapPending = false;

            InitializeHandler();
        }

        private void InitializeHandler()
        {
            if (_actionMap == null) return;

            _inputHandler?.Clear();
            _inputHandler = new PlayerInputHandler<InputEvent>(_actionMap);
            _inputHandler.Initialize();
        }

        private void OnEnable()
        {
            if (_inputHandler != null) return;
            InitializeHandler();
        }

        private void OnDisable()
        {
            _inputHandler?.Clear();
        }
    }
}
