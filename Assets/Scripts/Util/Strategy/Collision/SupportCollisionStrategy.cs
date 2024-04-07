using Assets.Scripts.MovementSystem;
using Assets.Scripts.Util.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util.Strategy.Collision
{
    internal class SupportCollisionStrategy : CollisionStrategy
    {
        private UnityEngine.Collision _cachedCollision = null;

        public override void HandleCollisionEnter(IEnumerable<ContactPoint> contacts)
        {
            Collider thisCollider = contacts.ElementAt(0).thisCollider;
            Collider otherCollider = contacts.ElementAt(0).otherCollider;
            SupportCollidersTracker supportTracker = thisCollider.GetComponent<SupportCollidersTracker>();

            if (supportTracker.ProvidesSupport(contacts))
            {
                supportTracker.AddSupport(otherCollider);
            }
        }

        public override void HandleCollisionStay(IEnumerable<ContactPoint> contacts)
        {
            Collider thisCollider = contacts.ElementAt(0).thisCollider;
            Collider otherCollider = contacts.ElementAt(0).otherCollider;
            SupportCollidersTracker supportTracker = thisCollider.GetComponent<SupportCollidersTracker>();

            if (supportTracker.SupportColliders.Contains(otherCollider))
            {
                if (!supportTracker.ProvidesSupport(contacts))
                {
                    supportTracker.RemoveSupport(otherCollider);
                }
            }
            else
            {
                if (supportTracker.ProvidesSupport(contacts))
                {
                    supportTracker.AddSupport(otherCollider);
                }
            }
        }

        public override void HandleCollisionExit(IEnumerable<ContactPoint> contacts)
        {
            Collider thisCollider = contacts.ElementAt(0).thisCollider;
            Collider otherCollider = contacts.ElementAt(0).otherCollider;
            SupportCollidersTracker supportTracker = thisCollider.GetComponent<SupportCollidersTracker>();

            if (supportTracker.SupportColliders.Contains(otherCollider))
            {
                supportTracker.RemoveSupport(otherCollider);
            }
        }
    }
}
