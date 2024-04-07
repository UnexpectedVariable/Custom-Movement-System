using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.Util.Visitor
{
    internal interface IVisitable<T>
    {
        void Accept(IVisitor<T> visitor);
    }
}
