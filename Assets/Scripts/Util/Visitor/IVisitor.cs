using Assets.Scripts.MovementSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util.Visitor
{
    internal interface IVisitor<T>
    {
        void Visit(T visitable);
    }
}
