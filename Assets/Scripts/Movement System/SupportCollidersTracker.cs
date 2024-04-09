using Assets.Scripts.Util.Observer;
using Assets.Scripts.Util.Visitor;
using System;
using System.Collections.Generic;
using UnityEngine;
using IObserved = Assets.Scripts.Util.Observer.IObserved<Assets.Scripts.MovementSystem.SupportCollidersTracker>;
using IObserver = Assets.Scripts.Util.Observer.IObserver<Assets.Scripts.MovementSystem.SupportCollidersTracker>;

namespace Assets.Scripts.MovementSystem
{
    [DisallowMultipleComponent]
    public class SupportCollidersTracker : ObservedMonoBehaviour<SupportCollidersTracker>
    {
        public List<Collider> SupportColliders { get; private set; }
        [SerializeField]
        [Range(0f, 180f)]
        private float _maxSupportAngle = 0f;

        private void Awake()
        {
            SupportColliders = new List<Collider>();
        }

        public bool ProvidesSupport(IEnumerable<ContactPoint> contacts)
        {
            foreach (var contact in contacts)
            {
                Vector3 innerNormal = contact.normal * -1;
                if (Vector3.Angle(UnityEngine.Physics.gravity, innerNormal) < _maxSupportAngle) return true;
            }
            return false;
        }

        public void AddSupport(Collider collider)
        {
            Debug.Log($"Collider {collider.gameObject.name} added as support for {gameObject.name}");
            SupportColliders.Add(collider);
            Notify();
        }

        public void RemoveSupport(Collider collider)
        {
            Debug.Log($"Collider {collider.gameObject.name} removed from supports of {gameObject.name}");
            SupportColliders.Remove(collider);
            Notify();
        }

        public override void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Handle(this);
            }
        }
    }
}
