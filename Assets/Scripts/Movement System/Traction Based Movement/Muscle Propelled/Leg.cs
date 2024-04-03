using Assets.Scripts.MovementSystem;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Movement_System.Traction_Based_Movement.Muscle_Propelled
{
    internal class Leg : ForceGenerator
    {
        [SerializeField]
        [Range(0f, 1000f)]
        private float _forceMagnitude = 0f;
        public override float ForceMagnitude => _forceMagnitude;
    }
}
