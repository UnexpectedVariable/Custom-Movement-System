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
            float scalingFactorSqr = Mathf.Pow(ForceMagnitude, 2) / direction.sqrMagnitude;
            Debug.Log($"{gameObject.name} is outputing force:" +
                $"\nScaling factor: {scalingFactorSqr}" +
                $"\nOutput: {direction * scalingFactorSqr}");
            return direction * scalingFactorSqr;
        }
    }
}
