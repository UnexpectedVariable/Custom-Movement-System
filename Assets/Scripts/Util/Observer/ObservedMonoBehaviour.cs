using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Util.Observer;

namespace Assets.Scripts.Util.Observer
{
    public abstract class ObservedMonoBehaviour<T> : MonoBehaviour, IObserved<T> where T : MonoBehaviour
    {
        protected List<IObserver<T>> _observers = new();

        public void Attach(IObserver<T> observer)
        {
            Debug.Log($"{gameObject.name} support tracker attached new observer");
            _observers.Add(observer);
        }
        public void Attach(ICollection<IObserver<T>> observers)
        {
            Debug.Log($"{gameObject.name} support tracker attached {observers.Count} new observers");
            _observers.AddRange(observers);
        }

        public void Detach(IObserver<T> observer)
        {
            Debug.Log($"{gameObject.name} support tracker detached observer");
            _observers.Remove(observer);
        }
        public void Detach(ICollection<IObserver<T>> observers)
        {
            Debug.Log($"{gameObject.name} support tracker detached {observers.Count} observers");
            foreach (var observer in observers)
            {
                _observers.Remove(observer);
            }
        }

        public abstract void Notify();
    }
}
