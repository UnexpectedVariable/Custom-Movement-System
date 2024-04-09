using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util.Strategy.Collision
{
    internal interface ICollisionStrategy
    {
        void HandleCollisionEnter(IEnumerable<ContactPoint> contacts);
        void HandleCollisionStay(IEnumerable<ContactPoint> contacts);
        void HandleCollisionExit(IEnumerable<ContactPoint> contacts);
    }
}
