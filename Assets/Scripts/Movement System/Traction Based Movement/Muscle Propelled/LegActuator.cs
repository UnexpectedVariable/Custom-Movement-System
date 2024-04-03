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
    internal class LegActuator : ForceGeneratorActuator, Util.Observer.IObserver<SupportCollidersTracker>
    {
        [SerializeField]
        private SupportCollidersTracker _collidersTracker = null;
        [SerializeField]
        private bool _isLegSupported = default;

        private void Start()
        {
            _actuationConditions = new List<bool>();
            _actuationConditions.ToList().Add(_isLegSupported);

            _collidersTracker.Attach(this);
        }

        public void Handle(SupportCollidersTracker observed)
        {
            _isLegSupported = observed.SupportColliders.Count > 0;
            _actuationConditions.ToList()[0] = _isLegSupported;
        }
    }
}
