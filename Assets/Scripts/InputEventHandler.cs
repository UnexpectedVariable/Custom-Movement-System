﻿using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal abstract class InputEventHandler<T> : IEventHandler<T> where T : InputEvent
    {
        public abstract void Initialize();
        public abstract void Clear();

        public void HandleEvent(object o, T eventData)
        {
            throw new NotImplementedException();
        }
    }
}
