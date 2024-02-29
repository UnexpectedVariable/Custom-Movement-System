using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    internal class PlayerInputHandler : MonoBehaviour, IInputHandler
    {
        [SerializeField]
        private PlayerInput _playerInput = null;
        EventBinding<MovementEvent> _movementEventBinding;

        private void FixedUpdate()
        {
            if (_playerInput.currentActionMap["Forward"].IsPressed()) EventBus<MovementEvent>.Raise(new MovementEvent { direction = transform.forward });
            if (_playerInput.currentActionMap["Backward"].IsPressed()) EventBus<MovementEvent>.Raise(new MovementEvent { direction = transform.forward * -1 });
            if (_playerInput.currentActionMap["Right"].IsPressed()) EventBus<MovementEvent>.Raise(new MovementEvent { direction = transform.right });
            if (_playerInput.currentActionMap["Left"].IsPressed()) EventBus<MovementEvent>.Raise(new MovementEvent { direction = transform.right * -1 });
        }

        public void HandleInput()
        {
            
        }

        public void HandleMovementEvent(object o, MovementEvent e)
        {
            Debug.Log($"{o.GetType().Name} Movement event received! Direction {e.direction}");
        }

        private void OnEnable()
        {
            _movementEventBinding = new EventBinding<MovementEvent>(HandleMovementEvent);
            EventBus<MovementEvent>.Register( _movementEventBinding );
        }

        private void OnDisable()
        {
            EventBus<MovementEvent>.Deregister( _movementEventBinding );
        }
    }
}
