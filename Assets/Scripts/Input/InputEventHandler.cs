using Assets.Scripts.Events;
using System;

namespace Assets.Scripts.Input
{
    internal abstract class InputEventHandler<T> : IEventHandler<T> where T : InputEvent
    {
        public event EventHandler<T> OnInput;
        public abstract void Initialize();
        public abstract void Clear();

        public void HandleEvent(object o, T eventData)
        {
            OnInput?.Invoke(o, eventData);
        }
    }
}
