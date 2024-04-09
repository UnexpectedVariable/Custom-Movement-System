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
    internal class SupportCollisionStrategy : ICollisionStrategy
    {
        public void HandleCollisionEnter(IEnumerable<ContactPoint> contacts)
        {
            Collider otherCollider;
            SupportCollidersTracker supportTracker;
            ExtractData(contacts, out otherCollider, out supportTracker);

            if (supportTracker.ProvidesSupport(contacts))
            {
                supportTracker.AddSupport(otherCollider);
            }
        }

        public void HandleCollisionStay(IEnumerable<ContactPoint> contacts)
        {
            Collider otherCollider;
            SupportCollidersTracker supportTracker;
            ExtractData(contacts, out otherCollider, out supportTracker);

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

        public void HandleCollisionExit(IEnumerable<ContactPoint> contacts)
        {
            Collider otherCollider;
            SupportCollidersTracker supportTracker;
            ExtractData(contacts, out otherCollider, out supportTracker);

            if (supportTracker.SupportColliders.Contains(otherCollider))
            {
                supportTracker.RemoveSupport(otherCollider);
            }
        }

        private static void ExtractData(IEnumerable<ContactPoint> contacts, out Collider otherCollider, out SupportCollidersTracker supportTracker)
        {
            Collider thisCollider = contacts.ElementAt(0).thisCollider;
            otherCollider = contacts.ElementAt(0).otherCollider;
            supportTracker = thisCollider.GetComponent<SupportCollidersTracker>();
        }
    }
}
