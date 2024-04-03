using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    internal interface IForceGeneratorActuator
    {
        public Vector3 Actuate(Vector3 direction);
    }
}
