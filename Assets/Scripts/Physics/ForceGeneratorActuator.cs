using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    internal abstract class ForceGeneratorActuator : MonoBehaviour, IForceGeneratorActuator
    {
        [SerializeField]
        protected ForceGenerator _generator;

        public Vector3 Actuate(Vector3 direction)
        {
            if (!CanBeActuated(EnumerateConditions())) return Vector3.zero;
            return _generator.OutputForce(direction);
        }

        public bool TryActuate(Vector3 direction, out Vector3 result)
        {
            result = Vector3.zero;
            if (!CanBeActuated(EnumerateConditions())) return false;
            result = _generator.OutputForce(direction);
            return true;
        }

        private bool CanBeActuated(IEnumerable<bool> predicate)
        {
            foreach (var item in predicate)
            {
                if (!item) return false;
            }
            return true;
        }

        protected abstract IEnumerable<bool> EnumerateConditions();
    }
}
