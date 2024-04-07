using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util.Strategy.Collision
{
    internal abstract class CollisionStrategy
    {
        public abstract void HandleCollisionEnter(IEnumerable<ContactPoint> contacts);
        public abstract void HandleCollisionStay(IEnumerable<ContactPoint> contacts);
        public abstract void HandleCollisionExit(IEnumerable<ContactPoint> contacts);
    }
}
