using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Events
{
    public interface IEvent
    {
    }

    public class InputEvent : IEvent
    {
        public InputAction.CallbackContext InputContext { get; set; }
    }

    public class CollisionEvent : IEvent
    {
        public Collision collision { get; set; }
    }
}
