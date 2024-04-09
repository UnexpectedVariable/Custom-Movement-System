using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util.Observer
{
    internal abstract class Observed<T> : IObserved<T> where T : Observed<T>
    {
        protected List<IObserver<T>> _observers = new();

        public virtual void Attach(IObserver<T> observer)
        {
            _observers.Add(observer);
        }
        public virtual void Attach(ICollection<IObserver<T>> observers)
        {
            _observers.AddRange(observers);
        }

        public virtual void Detach(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }
        public virtual void Detach(ICollection<IObserver<T>> observers)
        {
            foreach (var observer in observers)
            {
                _observers.Remove(observer);
            }
        }

        public abstract void Notify();
    }
}
