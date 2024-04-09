using Assets.Scripts.MovementSystem;
using Assets.Scripts.Physics.Friction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util.Strategy.Collision
{
    internal class FrictionCollisionStrategy : ICollisionStrategy
    {
        public void HandleCollisionEnter(IEnumerable<ContactPoint> contacts)
        {
            Handle(contacts);
        }

        public void HandleCollisionStay(IEnumerable<ContactPoint> contacts)
        {
            Handle(contacts);
        }

        public void HandleCollisionExit(IEnumerable<ContactPoint> contacts)
        {
            return;
        }

        private static void Handle(IEnumerable<ContactPoint> contacts)
        {
            Collider otherCollider;
            FrictionCollidersTracker frictionTracker;
            ExtractData(contacts, out otherCollider, out frictionTracker);

            frictionTracker.TryEnqueueCollision(otherCollider, contacts);
        }

        private static void ExtractData(IEnumerable<ContactPoint> contacts, out Collider otherCollider, out FrictionCollidersTracker frictionTracker)
        {
            Collider thisCollider = contacts.ElementAt(0).thisCollider;
            otherCollider = contacts.ElementAt(0).otherCollider;
            frictionTracker = thisCollider.GetComponent<FrictionCollidersTracker>();
        }
    }
}
