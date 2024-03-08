using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util.Observer
{
    public interface IObserver<T>
    {
        public void Handle(T observed);
    }
}
