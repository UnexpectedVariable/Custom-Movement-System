namespace Assets.Scripts.Events
{
    internal interface IEventHandler<T> : IHandler where T : IEvent
    {
        void Initialize();
        void Clear();
        void HandleEvent(object o, T eventData);
    }
}
