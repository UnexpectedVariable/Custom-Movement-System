using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dependency_Injection
{
    internal interface IInjectable<T> where T : class
    {
        void Inject(T instance);
    }
}
