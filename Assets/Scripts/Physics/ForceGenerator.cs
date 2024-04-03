using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    internal abstract class ForceGenerator : MonoBehaviour, IForceGenerator
    {
        public abstract float ForceMagnitude
        {
            get;
        }

        public virtual Vector3 OutputForce(Vector3 direction)
        {
            float scalingFactorSqr = direction.sqrMagnitude / Mathf.Pow(ForceMagnitude, 2);
            return direction * scalingFactorSqr;
        }
    }
}
