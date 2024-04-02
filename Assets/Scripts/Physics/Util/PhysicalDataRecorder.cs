using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Physics.Util
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    internal class PhysicalDataRecorder : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody = null;
        private PhysicalData _data = null;

        private Vector3 _lastVelocity = Vector3.zero;

        public PhysicalData Data
        {
            get => _data;
        }

        #region events
        /*public event EventHandler<Vector3> VelocityModified = null;
        public event EventHandler<Vector3> AnguilarVelocityModified = null;
        public event EventHandler<Vector3> CenterOfMassModified = null;

        public event EventHandler<float> DragModified = null;
        public event EventHandler<float> AngularDragModified = null;
        public event EventHandler<float> MassModified = null;*/
        public event EventHandler<Vector3> AccelerationChanged = null;
        #endregion

        private void Start()
        {
            if(_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
            _data = new PhysicalData(this);

            _lastVelocity = _rigidbody.velocity;
        }

        private void FixedUpdate()
        {
            UpdateAcceleration();
        }

        public void UpdateAcceleration()
        {
            Vector3 acceleration = 1 / Time.fixedDeltaTime * (_rigidbody.velocity - _lastVelocity);
            if (_data.Acceleration == acceleration) return;
            AccelerationChanged?.Invoke(this, acceleration);
        }
    }
}
