using System;

namespace Assets.Scripts.Events
{
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        event EventHandler<T> @event;

        event EventHandler<T> IEventBinding<T>.Event
        {
            add => @event += value;
            remove => @event -= value;
        }

        public EventBinding(EventHandler<T> @event)
        {
            this.@event = @event;
        }

        public void OnEvent(T eventArgs)
        {
            @event?.Invoke(this, eventArgs);
        }
    }
}
