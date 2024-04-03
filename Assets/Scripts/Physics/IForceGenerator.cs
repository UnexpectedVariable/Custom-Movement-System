using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    internal interface IForceGenerator
    {
        float ForceMagnitude { get; }
        Vector3 OutputForce(Vector3 direction);
    }
}
