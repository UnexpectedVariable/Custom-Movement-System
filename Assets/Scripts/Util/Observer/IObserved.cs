using System;
using System.Collections.Generic;

namespace Assets.Scripts.Util.Observer
{
    public interface IObserved<T>
    {
        public void Attach(IObserver<T> observer);
        public void Attach(ICollection<IObserver<T>> observers);

        public void Detach(IObserver<T> observer);
        public void Detach(ICollection<IObserver<T>> observers);

        public void Notify();
    }
}