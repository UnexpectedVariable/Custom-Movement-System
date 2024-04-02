using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Util
{
    internal class PhysicalData
    {
        /*public Vector3 Velocity { get; private set; }
        public Vector3 AngularVelocity { get; private set; }
        public Vector3 CenterOfMass { get; private set; }
        public float Drag { get; private set; }
        public float AngularDrag { get; private set; }
        public float Mass { get; private set; }
        public float Speed
        {
            get => Velocity.magnitude;
        }
        public float SqrSpeed
        {
            get => Velocity.sqrMagnitude;
        }*/
        /*public RigidbodyPhysicalData(Rigidbody rigidbody)
        {
            Velocity = rigidbody.velocity;
            AngularVelocity = rigidbody.angularVelocity;
            CenterOfMass = rigidbody.centerOfMass;
            Drag = rigidbody.drag;
            AngularDrag = rigidbody.angularDrag;
            Mass = rigidbody.mass;
        }*/
        public Vector3 Acceleration { get; private set; }

        public PhysicalData(PhysicalDataRecorder recorder)
        {
            recorder.AccelerationChanged += OnAccelerationChanged;
        }

        private void OnAccelerationChanged(object sender, Vector3 newAcceleration)
        {
            Acceleration = newAcceleration;
        }
    }
}
