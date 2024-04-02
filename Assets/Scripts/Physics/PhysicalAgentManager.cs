using Assets.Scripts.Physics.Util;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    [DisallowMultipleComponent]
    internal class PhysicalAgentManager : MonoBehaviour
    {
        public IDataRepository<PhysicalData> PhysicalDataRepository { get; private set; }

        private List<PhysicalDataRecorder> _dataRecorders = null;
        private List<Rigidbody> _rigidbodies = null;

        private float _totalMass = -1f;
        public float TotalMass
        {
            get
            {
                if (_totalMass < 0f) _totalMass = CalculateTotalMass();
                return _totalMass;
            }
        }
        public Vector3 AverageAcceleration
        {
            get
            {
                return CalculateAveragerAcceleration();
            }
        }

        public float CalculateTotalMass()
        {
            return Enumerable.Sum(_rigidbodies.Select((rigidbody) => rigidbody.mass));
        }

        public Vector3 CalculateAveragerAcceleration()
        {
            var data = PhysicalDataRepository.GetFullData();
            return data
                .Select((data) => data.Acceleration)
                .Sum() / data.Count();
        }

        private void Start()
        {
            _dataRecorders = GetComponentsInChildren<PhysicalDataRecorder>().ToList();
            _rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();

            PhysicalDataRepository = new PhysicalDataRepository();
            PhysicalDataRepository.AddBulkData(_dataRecorders.Select((recorder) => recorder.Data));
        }
    }
}
