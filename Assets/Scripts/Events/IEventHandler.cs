using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events
{
    internal interface IEventHandler<T> : IHandler where T : IEvent
    {
        void Initialize();
        void Clear();
        void HandleEvent(object o, T eventData);
    }
}
