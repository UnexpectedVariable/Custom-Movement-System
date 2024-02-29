using System;

namespace Assets.Scripts.Events
{
    internal interface IEventBinding<T>
    {
        public event EventHandler<T> Event;

        public void OnEvent(T eventArgs);
    }
}
