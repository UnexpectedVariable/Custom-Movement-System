using Assets.Scripts.Physics.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Friction
{
    internal static class FrictionInteractionCalculator
    {
        public static Vector3 CalculateSlippingForce((Collision collision, Surface surface) item, PhysicalAgentManager manager, Surface surface)
        {
            List<ContactPoint> contacts = new List<ContactPoint>(item.collision.contactCount);
            item.collision.GetContacts(contacts);

            float maxAngle = 0f;
            foreach (var contact in contacts)
            {
                var angle = Vector3.Angle(contact.normal, Vector3.forward);
                if (angle > maxAngle)
                {
                    maxAngle = angle;
                }
            }
            Vector3 tangentialForce = manager.TotalMass * manager.AverageAcceleration;
            Vector3 normalForce = manager.TotalMass * Mathf.Cos(maxAngle) * UnityEngine.Physics.gravity;
            Vector3 frictionForce = normalForce * PhysicsUtil.GetCoF((surface.Type, item.surface.Type));

            float frictionForceSqrMgnt = frictionForce.sqrMagnitude;
            float tangentialForceSqrMgnt = tangentialForce.sqrMagnitude;
            if (frictionForceSqrMgnt > tangentialForceSqrMgnt) return Vector3.zero;

            float slippingForceSqrMgnt = frictionForceSqrMgnt - tangentialForceSqrMgnt;
            float slippingForceProportion = tangentialForceSqrMgnt / slippingForceSqrMgnt;
            return tangentialForce / (float)Math.Sqrt(slippingForceProportion);
        }

        public static Vector3 CalculateSlippingForce(IEnumerable<ContactPoint> contacts, Surface slippingSurface, PhysicalAgentManager manager, Surface surface)
        {
            float maxAngle = 0f;
            foreach (var contact in contacts)
            {
                var angle = Vector3.Angle(contact.normal, Vector3.forward);
                if (angle > maxAngle)
                {
                    maxAngle = angle;
                }
            }
            Vector3 tangentialForce = manager.TotalMass * manager.AverageAcceleration;
            Vector3 normalForce = manager.TotalMass * Mathf.Cos(maxAngle) * UnityEngine.Physics.gravity;
            Vector3 frictionForce = normalForce * PhysicsUtil.GetCoF((surface.Type, slippingSurface.Type));

            float frictionForceSqrMgnt = frictionForce.sqrMagnitude;
            float tangentialForceSqrMgnt = tangentialForce.sqrMagnitude;
            if (frictionForceSqrMgnt > tangentialForceSqrMgnt) return Vector3.zero;

            float slippingForceSqrMgnt = frictionForceSqrMgnt - tangentialForceSqrMgnt;
            float slippingForceProportion = tangentialForceSqrMgnt / slippingForceSqrMgnt;
            return tangentialForce / (float)Math.Sqrt(slippingForceProportion);
        }
    }
}
